using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WPFBase
{
    public class Mediator : IMediator
    {
        private static List <Listener> listners = new List <Listener>();

        public Mediator ()
        {            
        }

        public void RegisterInterest(int msgID, Action<object> callback, object caller)
        {
            listners.Add(new Listener(){Msgid = msgID, Callback = callback, Caller=caller});
        }

        private class Listener
        {
            internal int Msgid {get; set;}
            internal Action<object> Callback{get; set;}
            internal object Caller{get; set;}

            internal Listener()
            {                
            }
        }


        public void InformChange(int msgID, object payload, object caller)
        {
            var targets = listners.Where(f => f.Msgid == msgID && f.Caller != caller);

            foreach (Listener t in targets.ToList())
            {
               t.Callback(payload);
            }

        }

    }
}
