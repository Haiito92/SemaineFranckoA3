namespace LittleGraph.Runtime
{
    [System.Serializable]
    public struct LGConnection
    {
        public LGConnectionPort InputPort;
        public LGConnectionPort OutputPort;

        public LGConnection(LGConnectionPort inputPort, LGConnectionPort outputPort)
        {
            InputPort = inputPort;
            OutputPort = outputPort;
        }

        public LGConnection(string inputNodeId, int inputPortIndex, string inputPortName, string outputNodeId, int outputPortIndex,  string outputPortName)
        {
            InputPort = new LGConnectionPort(inputNodeId, inputPortIndex, inputPortName);
            OutputPort = new LGConnectionPort(outputNodeId, outputPortIndex, outputPortName);
        }
    }
    
    [System.Serializable]
    public struct LGConnectionPort
    {
        public string NodeId;
        public int PortIndex;
        public string PortName;

        public LGConnectionPort(string nodeId, int portIndex, string portName)
        {
            NodeId = nodeId;
            PortIndex = portIndex;
            PortName = portName;
        }
    }
}
