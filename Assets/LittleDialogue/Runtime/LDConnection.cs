namespace LittleDialogue.Runtime
{
    [System.Serializable]
    public struct LDConnection
    {
        public LDConnectionPort InputPort;
        public LDConnectionPort OutputPort;

        public LDConnection(LDConnectionPort inputPort, LDConnectionPort outputPort)
        {
            InputPort = inputPort;
            OutputPort = outputPort;
        }

        public LDConnection(string inputNodeId, int inputPortIndex, string outputNodeId, int outputPortIndex)
        {
            InputPort = new LDConnectionPort(inputNodeId, inputPortIndex);
            OutputPort = new LDConnectionPort(outputNodeId, outputPortIndex);
        }
    }
    
    [System.Serializable]
    public struct LDConnectionPort
    {
        public string NodeId;
        public int PortIndex;

        public LDConnectionPort(string nodeId, int portIndex)
        {
            NodeId = nodeId;
            PortIndex = portIndex;
        }
    }
}
