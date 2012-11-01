using System;

namespace TripleTriadRefresh.Server.Framework.Aspects.Attributes
{
    public class GameInjectAttribute : Attribute
    {
        public int ParameterIndex { get; private set; }
        public int? KeyIndex { get; private set; }
        public string Error { get; private set; }

        public GameInjectAttribute(int parameterIndex, string error)
        {
            this.ParameterIndex = parameterIndex;
            this.Error = error;
        }

        public GameInjectAttribute(int parameterIndex, int keyIndex, string error)
        {
            this.ParameterIndex = parameterIndex;
            this.KeyIndex = keyIndex;
            this.Error = error;
        }
    }
}