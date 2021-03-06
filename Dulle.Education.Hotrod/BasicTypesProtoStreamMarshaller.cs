﻿
using Google.Protobuf;
using Dulle.Education.Hotrod;
using Infinispan.Hotrod.Protobuf;
using Infinispan.HotRod;
using Org.Infinispan.Protostream;
using System;

namespace Dulle.Education.Hotrod
{
    class BasicTypesProtoStreamMarshaller : IMarshaller
    {
        public bool IsMarshallable(object o)
        {
            throw new NotImplementedException();
        }


        public object ObjectFromByteBuffer(byte[] buf)
        {
            base_types bt = base_types.Parser.ParseFrom(buf);
            return bt;
            throw new NotImplementedException();
        }


        public object ObjectFromByteBuffer(byte[] buf, int offset, int length)
        {
            throw new NotImplementedException();
        }


        public byte[] ObjectToByteBuffer(object obj)
        {
            if (obj.GetType() == typeof(String))
            {
                return StringToByteBuffer((String)obj);
            }
            if (obj.GetType() == typeof(int))
            {
                return IntToByteBuffer((int)obj);
            }
            if (obj.GetType() == typeof(Person))
            {
                Person u = (Person)obj;

                int size = u.CalculateSize();
                byte[] bytes = new byte[size];
                CodedOutputStream cos = new CodedOutputStream(bytes);
                u.WriteTo(cos);
                cos.Flush();
                WrappedMessage wm = new WrappedMessage();
                wm.WrappedMessageBytes = ByteString.CopyFrom(bytes);
                wm.WrappedDescriptorId = 42;


                byte[] msgBytes = new byte[wm.CalculateSize()];
                CodedOutputStream msgCos = new CodedOutputStream(msgBytes);
                wm.WriteTo(msgCos);
                msgCos.Flush();
                return msgBytes;
            }


            throw new NotImplementedException();
        }


        private byte[] StringToByteBuffer(string str)
        {
            int t = CodedOutputStream.ComputeTagSize(9);
            int s = CodedOutputStream.ComputeStringSize(str);


            s += t;
            byte[] bytes = new byte[s];
            CodedOutputStream cos = new CodedOutputStream(bytes);
            cos.WriteTag((9 << 3) + 2);
            cos.WriteString(str);
            cos.Flush();
            return bytes;
        }


        private byte[] IntToByteBuffer(int i)
        {
            int t = CodedOutputStream.ComputeTagSize(5);
            int s = CodedOutputStream.ComputeInt32Size(i);


            s += t;
            byte[] bytes = new byte[s];
            CodedOutputStream cos = new CodedOutputStream(bytes);
            cos.WriteTag((5 << 3) + 0);
            cos.WriteInt32(i);
            cos.Flush();
            return bytes;
        }




        public byte[] ObjectToByteBuffer(object obj, int estimatedSize)
        {
            throw new NotImplementedException();
        }
    }
}
