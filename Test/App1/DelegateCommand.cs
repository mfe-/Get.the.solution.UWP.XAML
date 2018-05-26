using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Windows.Input;

using System.Threading;
using System.Reflection;

namespace App1
{
    internal class DelegateCommand<T> : ICommand
    {
        readonly Action<T> _executeMethod;
        Func<T, bool> _canExecuteMethod;
        private bool _isActive;

        private SynchronizationContext _synchronizationContext;
        private readonly HashSet<string> _observedPropertiesExpressions = new HashSet<string>();

        /// <summary>
        /// Initializes a new instance of <see cref="DelegateCommand{T}"/>.
        /// </summary>
        /// <param name="executeMethod">Delegate to execute when Execute is called on the command. This can be null to just hook up a CanExecute delegate.</param>
        /// <remarks><see cref="CanExecute"/> will always return true.</remarks>
        public DelegateCommand(Action<T> executeMethod)
            : this(executeMethod, (o) => true)
        {
            _synchronizationContext = SynchronizationContext.Current;
        }


        /// <summary>
        /// Initializes a new instance of <see cref="DelegateCommand{T}"/>.
        /// </summary>
        /// <param name="executeMethod">Delegate to execute when Execute is called on the command. This can be null to just hook up a CanExecute delegate.</param>
        /// <param name="canExecuteMethod">Delegate to execute when CanExecute is called on the command. This can be null.</param>
        /// <exception cref="ArgumentNullException">When both <paramref name="executeMethod"/> and <paramref name="canExecuteMethod"/> ar <see langword="null" />.</exception>
        public DelegateCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod)
            : base()
        {
            if (executeMethod == null || canExecuteMethod == null)
                throw new ArgumentNullException(nameof(executeMethod), "Resources.DelegateCommandDelegatesCannotBeNull");

            TypeInfo genericTypeInfo = typeof(T).GetTypeInfo();

            // DelegateCommand allows object or Nullable<>.  
            // note: Nullable<> is a struct so we cannot use a class constraint.
            if (genericTypeInfo.IsValueType)
            {
                if ((!genericTypeInfo.IsGenericType) || (!typeof(Nullable<>).GetTypeInfo().IsAssignableFrom(genericTypeInfo.GetGenericTypeDefinition().GetTypeInfo())))
                {
                    throw new InvalidCastException("Resources.DelegateCommandInvalidGenericPayloadType");
                }
            }

            _executeMethod = executeMethod;
            _canExecuteMethod = canExecuteMethod;
        }

        ///<summary>
        ///Executes the command and invokes the <see cref="Action{T}"/> provided during construction.
        ///</summary>
        ///<param name="parameter">Data used by the command.</param>
        public void Execute(object parameter)
        {
            _executeMethod((T)parameter);
        }

        ///<summary>
        ///Determines if the command can execute by invoked the <see cref="Func{T,Bool}"/> provided during construction.
        ///</summary>
        ///<param name="parameter">Data used by the command to determine if it can execute.</param>
        ///<returns>
        ///<see langword="true" /> if this command can be executed; otherwise, <see langword="false" />.
        ///</returns>
        public bool CanExecute(object parameter)
        {
            return _canExecuteMethod((T)parameter);
        }

        /// <summary>
        /// Observes a property that implements INotifyPropertyChanged, and automatically calls DelegateCommandBase.RaiseCanExecuteChanged on property changed notifications.
        /// </summary>
        /// <typeparam name="TType">The type of the return value of the method that this delegate encapulates</typeparam>
        /// <param name="propertyExpression">The property expression. Example: ObservesProperty(() => PropertyName).</param>
        /// <returns>The current instance of DelegateCommand</returns>
        public DelegateCommand<T> ObservesProperty<TType>(Expression<Func<TType>> propertyExpression)
        {
            //ObservesPropertyInternal(propertyExpression);
            return this;
        }

        /// <summary>
        /// Observes a property that is used to determine if this command can execute, and if it implements INotifyPropertyChanged it will automatically call DelegateCommandBase.RaiseCanExecuteChanged on property changed notifications.
        /// </summary>
        /// <param name="canExecuteExpression">The property expression. Example: ObservesCanExecute(() => PropertyName).</param>
        /// <returns>The current instance of DelegateCommand</returns>
        public DelegateCommand<T> ObservesCanExecute(Expression<Func<bool>> canExecuteExpression)
        {
            Expression<Func<T, bool>> expression = Expression.Lambda<Func<T, bool>>(canExecuteExpression.Body, Expression.Parameter(typeof(T), "o"));
            _canExecuteMethod = expression.Compile();
            //ObservesPropertyInternal(canExecuteExpression);
            return this;
        }

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public virtual event EventHandler CanExecuteChanged;

        /// <summary>
        /// Raises <see cref="ICommand.CanExecuteChanged"/> so every 
        /// command invoker can requery <see cref="ICommand.CanExecute"/>.
        /// </summary>
        protected virtual void OnCanExecuteChanged()
        {
            var handler = CanExecuteChanged;
            if (handler != null)
            {
                if (_synchronizationContext != null && _synchronizationContext != SynchronizationContext.Current)
                    _synchronizationContext.Post((o) => handler.Invoke(this, EventArgs.Empty), null);
                else
                    handler.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Raises <see cref="DelegateCommandBase.CanExecuteChanged"/> so every command invoker
        /// can requery to check if the command can execute.
        /// <remarks>Note that this will trigger the execution of <see cref="DelegateCommandBase.InvokeCanExecute"/> once for each invoker.</remarks>
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
        public void RaiseCanExecuteChanged()
        {
            OnCanExecuteChanged();
        }


        /// <summary>
        /// Observes a property that implements INotifyPropertyChanged, and automatically calls DelegateCommandBase.RaiseCanExecuteChanged on property changed notifications.
        /// </summary>
        /// <typeparam name="T">The object type containing the property specified in the expression.</typeparam>
        /// <param name="propertyExpression">The property expression. Example: ObservesProperty(() => PropertyName).</param>
        protected internal void ObservesPropertyInternal<T>(Expression<Func<T>> propertyExpression)
        {
            if (_observedPropertiesExpressions.Contains(propertyExpression.ToString()))
            {
                throw new ArgumentException($"{propertyExpression.ToString()} is already being observed.",
                    nameof(propertyExpression));
            }
            else
            {
                _observedPropertiesExpressions.Add(propertyExpression.ToString());
                //PropertyObserver.Observes(propertyExpression, RaiseCanExecuteChanged);
            }
        }

        #region IsActive

        /// <summary>
        /// Gets or sets a value indicating whether the object is active.
        /// </summary>
        /// <value><see langword="true" /> if the object is active; otherwise <see langword="false" />.</value>
        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                if (_isActive != value)
                {
                    _isActive = value;
                    OnIsActiveChanged();
                }
            }
        }

        /// <summary>
        /// Fired if the <see cref="IsActive"/> property changes.
        /// </summary>
        public virtual event EventHandler IsActiveChanged;

        /// <summary>
        /// This raises the <see cref="DelegateCommandBase.IsActiveChanged"/> event.
        /// </summary>
        protected virtual void OnIsActiveChanged()
        {
            IsActiveChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion

    }
}