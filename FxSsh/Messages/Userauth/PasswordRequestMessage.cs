﻿using System;
using System.Text;

namespace FxSsh.Messages.Userauth
{
    public class PasswordRequestMessage : RequestMessage
    {
        public string Password { get; private set; }

        protected override void OnLoad(SshDataWorker reader)
        {
            base.OnLoad(reader);

            if (MethodName != "password")
                throw new ArgumentException(string.Format("Method name {0} is not valid.", MethodName));

            var falseField = reader.ReadBoolean();
            Password = reader.ReadString(Encoding.UTF8);
        }
    }
}