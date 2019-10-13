/*
This file is part of Super Pong.

Super Pong is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Super Pong is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Super Pong.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Collections.Generic;
using Events.Exceptions;

namespace Events
{
    public class EventManager
    {
        public static EventManager Instance { get; } = new EventManager();

        Dictionary<Type, List<WeakReference<IEventListener>>> _listeners = new Dictionary<Type, List<WeakReference<IEventListener>>>();
        List<IEvent> _queue = new List<IEvent>();

        public void RegisterListener<T>(IEventListener listener) where T : IEvent
        {
            RegisterListener(typeof(T), listener);
        }

        public void RegisterListener(Type type, IEventListener listener)
        {
            if (!type.IsEvent())
            {
                throw new TypeNotEventException();
            }

            EnsureInitiatedListener(type);

            if (ContainsListener(type, listener))
            {
                throw new ListenerAlreadyExistsException();
            }

            _listeners[type].Add(new WeakReference<IEventListener>(listener));
        }

        public void UnregisterListener(IEventListener listener)
        {
            foreach (Type key in _listeners.Keys)
            {
                UnregisterListener(key, listener);
            }
        }

        public bool UnregisterListener<T>(IEventListener listener) where T : IEvent
        {
            return UnregisterListener(typeof(T), listener);
        }

        public bool UnregisterListener(Type type, IEventListener listener)
        {
            if (!type.IsEvent())
            {
                throw new TypeNotEventException();
            }

            EnsureInitiatedListener(type);

            foreach (WeakReference<IEventListener> listenerRef in _listeners[type])
            {
                IEventListener checkListener;
                listenerRef.TryGetTarget(out checkListener);
                if (checkListener != null)
                {
                    if (checkListener == listener)
                    {
                        _listeners[type].Remove(listenerRef);
                        return true;
                    }
                }
            }

            return false;
        }

        private bool ContainsListener(Type type, IEventListener listener)
        {
            foreach (WeakReference<IEventListener> listenerRef in _listeners[type])
            {
                IEventListener checkListener;
                listenerRef.TryGetTarget(out checkListener);
                if(checkListener != null)
                {
                    if(checkListener == listener)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public void QueueEvent(IEvent evt)
        {
            _queue.Add(evt);
        }

        public void Dispatch()
        {
            while (_queue.Count > 0)
            {
                TriggerEvent(_queue[0]);
                _queue.RemoveAt(0);
            }
        }

        public bool TriggerEvent(IEvent evt)
        {
            EnsureInitiatedListener(evt.GetType());

            for (int i = _listeners[evt.GetType()].Count - 1; i >= 0; i--)
            {
                WeakReference<IEventListener> listenerRef = _listeners[evt.GetType()][i];
                IEventListener listener;
                listenerRef.TryGetTarget(out listener);
                if(listener == null)
                {
                    UnregisterListener(listener);
                    continue;
                }
                if (listener.Handle(evt))
                {
                    return true;
                }
            }

            return false;
        }

        void EnsureInitiatedListener(Type type)
        {
            if (!_listeners.ContainsKey(type))
            {
                _listeners.Add(type, new List<WeakReference<IEventListener>>());
            }
        }

    }
}
