﻿using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace Plugin.SharedTransitions
{
    /// <summary>
    /// Shell with support for shared transitions
    /// </summary>
    /// <seealso cref="Xamarin.Forms.Shell" />
    public class SharedTransitionShell : Shell, ISharedTransitionContainer
    {
        /// <summary>
        /// Map for all transitions (and support properties) associated with this SharedTransitionShell
        /// </summary>
        internal static readonly BindablePropertyKey TransitionMapPropertyKey =
            BindableProperty.CreateReadOnly("TransitionMap", typeof(ITransitionMapper), typeof(SharedTransitionShell), default(ITransitionMapper));

        public static readonly BindableProperty TransitionMapProperty = TransitionMapPropertyKey.BindableProperty;

        /// <summary>
        /// The shared transition selected group for dynamic transitions
        /// </summary>
        public static readonly BindableProperty TransitionSelectedGroupProperty =
            BindableProperty.CreateAttached("TransitionSelectedGroup", typeof(string), typeof(SharedTransitionShell), null);

        /// <summary>
        /// The background animation associated with the current page in stack
        /// </summary>
        public static readonly BindableProperty BackgroundAnimationProperty =
            BindableProperty.CreateAttached("BackgroundAnimation", typeof(BackgroundAnimation), typeof(SharedTransitionShell), BackgroundAnimation.None);

        /// <summary>
        /// The shared transition duration (in ms) associated with the current page in stack
        /// </summary>
        public static readonly BindableProperty TransitionDurationProperty =
            BindableProperty.CreateAttached("TransitionDuration", typeof(long), typeof(SharedTransitionShell), (long)300);

        public event EventHandler TransitionStarted;
        public event EventHandler TransitionEnded;
        public event EventHandler TransitionCancelled;

        internal ITransitionMapper TransitionMap
        {
	        get => (ITransitionMapper)GetValue(TransitionMapProperty);
	        set => SetValue(TransitionMapPropertyKey, value);
        }

        /// <summary>
        /// Gets the transition map
        /// </summary>
        /// <value>
        /// The transition map
        /// </value>
        ITransitionMapper ISharedTransitionContainer.TransitionMap
        {
	        get => TransitionMap;
	        set => TransitionMap = value;
        }

        public SharedTransitionShell() => TransitionMap = new TransitionMapper();

        /// <summary>
        /// Gets the transition selected group
        /// </summary>
        public static string GetTransitionSelectedGroup(Page page)
        {
            return (string)page.GetValue(TransitionSelectedGroupProperty);
        }

        /// <summary>
        /// Gets the background animation.
        /// </summary>
        public static BackgroundAnimation GetBackgroundAnimation(Page page)
        {
            return (BackgroundAnimation)page.GetValue(BackgroundAnimationProperty);
        }

        /// <summary>
        /// Gets the duration of the shared transition
        /// </summary>
        public static long GetTransitionDuration(Page page)
        {
            return (long)page.GetValue(TransitionDurationProperty);
        }

        /// <summary>
        /// Sets the transition selected group
        /// </summary>
        public static void SetTransitionSelectedGroup(Page page, string value)
        {
            page.SetValue(TransitionSelectedGroupProperty, value);
        }

        /// <summary>
        /// Sets the background animation
        /// </summary>
        public static void SetBackgroundAnimation(Page page, BackgroundAnimation value)
        {
            page.SetValue(BackgroundAnimationProperty, value);
        }

        /// <summary>
        /// Sets the duration of the shared transition
        /// </summary>
        public static void SetTransitionDuration(Page page, long value)
        {
            page.SetValue(TransitionDurationProperty, value);
        }

        public virtual void OnTransitionStarted(){ }
        public virtual void OnTransitionEnded(){ }
        public virtual void OnTransitionCancelled(){ }


        [EditorBrowsable(EditorBrowsableState.Never)]
        public void SendTransitionStarted()
        {
            TransitionStarted?.Invoke(this, null);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public void SendTransitionEnded()
        {
            TransitionEnded?.Invoke(this, null);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public void SendTransitionCancelled()
        {
            TransitionCancelled?.Invoke(this, null);
        }

        protected override void OnChildRemoved(Element child)
        {
            TransitionMap.RemoveFromPage((Page)child);
        }
    }
}
