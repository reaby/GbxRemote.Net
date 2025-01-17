﻿using GbxRemoteNet.XmlRpc.Packets;
using GbxRemoteNet.XmlRpc.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GbxRemote.Net.Tests.XmlRpcTests.PacketsTests {
    public class MethodCallTests : IClassFixture<MessageFixture> {
        MessageFixture fixture;

        public MethodCallTests(MessageFixture fixture) {
            this.fixture = fixture;
        }

        [Fact]
        public void Correctly_Parses_Callback_Message_With_String_And_Integer_Args() {
            ResponseMessage msg = fixture.GetMessage(0x1, fixture.MethodCallString);
            MethodCall methodCall = new(msg);

            string arg1 = ((XmlRpcString)methodCall.Arguments[0]).Value;
            int arg2 = ((XmlRpcInteger)methodCall.Arguments[1]).Value;

            Assert.Equal("Example.Method.Name", methodCall.Method);
            Assert.Equal("Example Value 1", arg1);
            Assert.Equal(42, arg2);
        }

        [Fact]
        public void Correctly_Serializes_Call_With_String_And_Integer_Args() {
            MethodCall call = new("Example.Method.Name", 0x80000001,
                new XmlRpcBaseType[] {
                    new XmlRpcString("Example Value 1"),
                    new XmlRpcInteger(42)
                }
            );

            byte[] serialized = call.Serialize().GetAwaiter().GetResult();

            byte[] checkBytes = new byte[] {
                0x42, 0x1, 0x0, 0x0,  0x1, 0x0, 0x0, 0x80,
                0x3c,0x3f,0x78,0x6d,0x6c,0x20,0x76,0x65,0x72,0x73,0x69,0x6f,0x6e,0x3d,0x22,0x31,0x2e,0x30,0x22,0x20,
                0x65,0x6e,0x63,0x6f,0x64,0x69,0x6e,0x67,0x3d,0x22,0x75,0x74,0x66,0x2d,0x31,0x36,0x22,0x3f,0x3e,0x0d,
                0x0a,0x3c,0x6d,0x65,0x74,0x68,0x6f,0x64,0x43,0x61,0x6c,0x6c,0x3e,0x0d,0x0a,0x20,0x20,0x3c,0x6d,0x65,
                0x74,0x68,0x6f,0x64,0x4e,0x61,0x6d,0x65,0x3e,0x45,0x78,0x61,0x6d,0x70,0x6c,0x65,0x2e,0x4d,0x65,0x74,
                0x68,0x6f,0x64,0x2e,0x4e,0x61,0x6d,0x65,0x3c,0x2f,0x6d,0x65,0x74,0x68,0x6f,0x64,0x4e,0x61,0x6d,0x65,
                0x3e,0x0d,0x0a,0x20,0x20,0x3c,0x70,0x61,0x72,0x61,0x6d,0x73,0x3e,0x0d,0x0a,0x20,0x20,0x20,0x20,0x3c,
                0x70,0x61,0x72,0x61,0x6d,0x3e,0x0d,0x0a,0x20,0x20,0x20,0x20,0x20,0x20,0x3c,0x76,0x61,0x6c,0x75,0x65,
                0x3e,0x0d,0x0a,0x20,0x20,0x20,0x20,0x20,0x20,0x20,0x20,0x3c,0x73,0x74,0x72,0x69,0x6e,0x67,0x3e,0x45,
                0x78,0x61,0x6d,0x70,0x6c,0x65,0x20,0x56,0x61,0x6c,0x75,0x65,0x20,0x31,0x3c,0x2f,0x73,0x74,0x72,0x69,
                0x6e,0x67,0x3e,0x0d,0x0a,0x20,0x20,0x20,0x20,0x20,0x20,0x3c,0x2f,0x76,0x61,0x6c,0x75,0x65,0x3e,0x0d,
                0x0a,0x20,0x20,0x20,0x20,0x3c,0x2f,0x70,0x61,0x72,0x61,0x6d,0x3e,0x0d,0x0a,0x20,0x20,0x20,0x20,0x3c,
                0x70,0x61,0x72,0x61,0x6d,0x3e,0x0d,0x0a,0x20,0x20,0x20,0x20,0x20,0x20,0x3c,0x76,0x61,0x6c,0x75,0x65,
                0x3e,0x0d,0x0a,0x20,0x20,0x20,0x20,0x20,0x20,0x20,0x20,0x3c,0x69,0x6e,0x74,0x3e,0x34,0x32,0x3c,0x2f,
                0x69,0x6e,0x74,0x3e,0x0d,0x0a,0x20,0x20,0x20,0x20,0x20,0x20,0x3c,0x2f,0x76,0x61,0x6c,0x75,0x65,0x3e,
                0x0d,0x0a,0x20,0x20,0x20,0x20,0x3c,0x2f,0x70,0x61,0x72,0x61,0x6d,0x3e,0x0d,0x0a,0x20,0x20,0x3c,0x2f,
                0x70,0x61,0x72,0x61,0x6d,0x73,0x3e,0x0d,0x0a,0x3c,0x2f,0x6d,0x65,0x74,0x68,0x6f,0x64,0x43,0x61,0x6c,
                0x6c,0x3e
            };

            Assert.Equal(checkBytes, serialized);
        }
    }
}
