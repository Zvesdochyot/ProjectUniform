namespace UniformQuoridor.Controller
{
    public class Controller
    {
        private const char PartsSeparator = ' ';
        
        private const string MoveCommand = "move";
        private const string PlaceCommand = "place";

        public void AcceptRequest(string input)
        {
            var divided = input.Split(PartsSeparator);

            if (divided.Length != 2)
            {
                // TODO: throw an error on wrong input
            }
            
            ParseAction(divided[0], divided[1]);
        }

        public void ParseAction(string command, string argument)
        {
            switch (command.ToLower())
            {
                case MoveCommand:
                    break;
                case PlaceCommand:
                    break;
                default:
                    // TODO: throw an error on wrong input
                    break;
            }
        }
        
        
    }
}
