module DatingConsole {
    
    export class Dating {
        
        public static Main(args: string[]) {
            let Me: Person = new Person("Michael");
            let You: Person = new Person();
            for (let Girl in You.GirlFriends) {
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
                    catch (rx /*:RelationshipExistException*/) {
                        throw rx.ReTry();
                    }
                    
                }
                
            }
            
        }
    }
    
    // Implement yours
    export class Person {
        
        private v: string;
        
        public get GirlFriend(): Object {
        }
        public set GirlFriend(value: Object)  {
        }
        
        public get GirlFriends(): IEnumerable<Object> {
        }
        public set GirlFriends(value: IEnumerable<Object>)  {
        }
        
        public get Happy(): boolean {
        }
        public set Happy(value: boolean)  {
        }
        
        // Parameterless Constuct
        public constructor () {
            
        }
        
        public constructor (v: string) {
            this.v = this.v;
        }
        
        private /* internal */ AskGirlOut(girl: Object): string {
            throw new NotImplementedException();
        }
        
        private /* internal */ CrushOn(girl: Object): boolean {
            throw new NotImplementedException();
        }
        
        private /* internal */ EnjoyLife() {
            throw new NotImplementedException();
        }
        
        private /* internal */ PrepareForMarriage() {
            throw new NotImplementedException();
        }
        
        private /* internal */ ReTry() {
            throw new NotImplementedException();
        }
    }
    
    // Implement your exception
    @Serializable()
    class RelationshipExistException extends Exception {
        
        public constructor () {
            
        }
        
        public constructor (message: string) : 
                base(message) {
            
        }
        
        public constructor (message: string, innerException: Exception) : 
                base(message, innerException) {
            
        }
        
        protected constructor (info: SerializationInfo, context: StreamingContext) : 
                base(info, context) {
            
        }
        
        private /* internal */ ReTry(): Exception {
            throw new NotImplementedException();
        }
    }
}