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

        public LGConnection(string inputNodeId, int inputPortIndex, object inputPortData, string outputNodeId, int outputPortIndex, object outputPortData)
        {
            InputPort = new LGConnectionPort(inputNodeId, inputPortIndex, inputPortData);
            OutputPort = new LGConnectionPort(outputNodeId, outputPortIndex, outputPortData);
        }
    }
    
    [System.Serializable]
    public struct LGConnectionPort
    {
        public string NodeId;
        public int PortIndex;
        public object PortData;

        public LGConnectionPort(string nodeId, int portIndex, object portData)
        {
            NodeId = nodeId;
            PortIndex = portIndex;
            PortData = portData;
        }
    }
}
