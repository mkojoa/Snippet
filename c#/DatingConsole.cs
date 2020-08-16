using System;
using System.Collections.Generic;
using System.IO;
using DatingImplementation;


namespace DatingConsole
{
    public class Dating
    {

        /* Dating Algorithm */
        public static void Main(string[] args)
        {
            Person Me = new Person("Michael");
            Person You = new Person();

            /* Loop through all the girls in the church */
            foreach (var Girl in You.GirlFriends)
            {
                /*if i dont have a girlfriend but am currently crushing on any. */
                if (Me.GirlFriend == null && Me.CrushOn(Girl))
                {
                    /* try and ask the girl out */
                    try
                    {
                        /* If she accepts my proposal */
                        if (Me.AskGirlOut(Girl) == "Accepted")
                        {
                            Me.Happy = true;
                            Me.EnjoyLife();
                        }
                        else
                        {
                            /* try a different girl. */
                            Me.ReTry();
                        }
                    }
                    catch (RelationshipExistException rx)
                    {
                        /* catch your dating mistakes and try again */
                        throw rx.ReTry();
                    }
                }
            }
        }




        public class Person
        {
            private string v;

            public Person()
            {

            }

            public Person(string v)
            {
                this.v = v;
            }

            public object GirlFriend { get; internal set; }

            public IEnumerable<object> GirlFriends { get; internal set; }

            public bool Happy { get; internal set; }

            internal string AskGirlOut(object girl)
            {
                throw new NotImplementedException();
            }

            internal bool CrushOn(object girl)
            {
                throw new NotImplementedException();
            }

            internal void EnjoyLife()
            {
                throw new NotImplementedException();
            }

            internal void PrepareForMarriage()
            {
                throw new NotImplementedException();
            }

            internal void ReTry()
            {
                throw new NotImplementedException();
            }
        }
    }



    ////Implement yours
    //namespace DatingImplementation
    //{
    //    public class Person
    //    {
    //        private string v;

    //        public Person()
    //        {

    //        }

    //        public Person(string v)
    //        {
    //            this.v = v;
    //        }

    //        public object GirlFriend { get; internal set; }

    //        public IEnumerable<object> GirlFriends { get; internal set; }

    //        public bool Happy { get; internal set; }

    //        internal string AskGirlOut(object girl)
    //        {
    //            throw new NotImplementedException();
    //        }

    //        internal bool CrushOn(object girl)
    //        {
    //            throw new NotImplementedException();
    //        }

    //        internal void EnjoyLife()
    //        {
    //            throw new NotImplementedException();
    //        }

    //        internal void PrepareForMarriage()
    //        {
    //            throw new NotImplementedException();
    //        }

    //        internal void ReTry()
    //        {
    //            throw new NotImplementedException();
    //        }
    //    }
    //}


    //Implement your exception
    [Serializable]
    internal class RelationshipExistException : Exception
    {
        public RelationshipExistException()
        {
        }

        public RelationshipExistException(string message) : base(message)
        {
        }

        public RelationshipExistException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected RelationshipExistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        internal Exception ReTry()
        {
            throw new NotImplementedException();
        }
    }
}