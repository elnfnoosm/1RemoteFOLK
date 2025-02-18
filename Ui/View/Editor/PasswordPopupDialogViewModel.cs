﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _1RM.Model;
using _1RM.Model.Protocol;
using _1RM.Model.Protocol.Base;
using _1RM.Service;
using _1RM.Utils;
using Shawn.Utils;
using Shawn.Utils.Wpf;

namespace _1RM.View.Editor
{
    public class PasswordPopupDialogViewModel : NotifyPropertyChangedBaseScreen
    {
        public static string LastUsername = "";
        public static string LastPassword = "";

        //public List<ProtocolBaseViewModel> ProtocolList { get; }

        public bool DialogResult { get; set; } = false;

        public string Title { get; set; } = "";

        private string _userName = "Administrator";
        public string UserName
        {
            get => _userName;
            set => SetAndNotifyIfChanged(ref _userName, value);
        }

        private string _password = "";
        public string Password
        {
            get => _password;
            set => SetAndNotifyIfChanged(ref _password, value);
        }

        public PasswordPopupDialogViewModel()
        {
            UserName = UnSafeStringEncipher.SimpleDecrypt(LastUsername) ?? LastUsername;
            Password = UnSafeStringEncipher.SimpleDecrypt(LastPassword) ?? LastPassword;
        }

        protected override void OnViewLoaded()
        {
            base.OnViewLoaded();
            if (View is PasswordPopupDialogView v)
            {
                v.TbUserName.Text = UserName;
                v.TbPwd.Password = Password;

                if (!string.IsNullOrEmpty(v.TbUserName.Text))
                {
                    v.TbPwd.Focus();
                }
                else
                {
                    v.TbUserName.Focus();
                    v.TbUserName.CaretIndex = v.TbUserName.Text.Length;
                }
            }
        }


        /// <summary>
        /// validate whether all fields are correct to save
        /// </summary>
        /// <returns></returns>
        public virtual bool CanSave()
        {
            if (!string.IsNullOrEmpty(UserName))
            {
                return true;
            }
            return false;
        }


        private RelayCommand? _cmdSave;
        public RelayCommand CmdSave
        {
            get
            {
                return _cmdSave ??= new RelayCommand((o) =>
                {
                    LastUsername = UnSafeStringEncipher.SimpleEncrypt(UserName);
                    LastPassword = UnSafeStringEncipher.SimpleEncrypt(Password);
                    this.DialogResult = true;
                    this.RequestClose(true);
                }, o => CanSave());
            }
        }


        private RelayCommand? _cmdQuit;
        public RelayCommand CmdQuit
        {
            get
            {
                return _cmdQuit ??= new RelayCommand((o) =>
                {
                    this.DialogResult = false;
                    this.RequestClose(false);
                });
            }
        }
    }
}
