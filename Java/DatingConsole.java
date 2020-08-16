namespace DatingConsole {
    
    public class Dating {
        
        public static void Main(string[] args) {
            Person Me = new Person("Michael");
            Person You = new Person();
            for (var Girl : You.GirlFriends) {
                if (((Me.GirlFriend == null) 
                            && Me.CrushOn(Girl))) {
                    try {
                        if ((Me.AskGirlOut(Girl) == "Accepted")) {
                            Me.Happy = true;
                            Me.EnjoyLife();
                        }
                        else {
                            Me.ReTry();
                        }
                        
                    }
                    catch (RelationshipExistException rx) {
                        throw rx.ReTry();
                    }
                    
                }
                
            }
            
        }
    }
    
    // Implement yours
    public class Person {
        
        private string v;
        
        public final object GirlFriend {
            get {
            }
            set {
            }
        }
        
        public final IEnumerable<object> GirlFriends {
            get {
            }
            set {
            }
        }
        
        public final boolean Happy {
            get {
            }
            set {
            }
        }
        
        // Parameterless Constuct
        public Person() {
            
        }
        
        public Person(string v) {
            this.v = this.v;
        }
        
        internal final string AskGirlOut(object girl) {
            throw new NotImplementedException();
        }
        
        internal final boolean CrushOn(object girl) {
            throw new NotImplementedException();
        }
        
        internal final void EnjoyLife() {
            throw new NotImplementedException();
        }
        
        internal final void PrepareForMarriage() {
            throw new NotImplementedException();
        }
        
        internal final void ReTry() {
            throw new NotImplementedException();
        }
    }
    
    // Implement your exception
    @Serializable()
    class RelationshipExistException extends Exception {
        
        public RelationshipExistException() {
            
        }
        
        public RelationshipExistException(string message) {
            super(message);
            
            
        }
        
        public RelationshipExistException(string message, Exception innerException) {
            super(message, innerException);
            
            
        }
        
        protected RelationshipExistException(SerializationInfo info, StreamingContext context) {
            super(info, context);
            
            
        }
        
        internal final Exception ReTry() {
            throw new NotImplementedException();
        }
    }
}