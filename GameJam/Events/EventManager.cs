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
        static EventManager instance = new EventManager();
        public static EventManager Instance
        {
            get
            {
                return instance;
            }
        }

        Dictionary<Type, List<IEventListener>> _listeners = new Dictionary<Type, List<IEventListener>>();
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

            if (_listeners[type].Contains(listener))
            {
                throw new ListenerAlreadyExistsException();
            }

            _listeners[type].Add(listener);
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

            return _listeners[type].Remove(listener);
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
                IEventListener listener = _listeners[evt.GetType()][i];
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
                _listeners.Add(type, new List<IEventListener>());
            }
        }

    }
}
