﻿/*
 * Copyright (c) 2023 Proton AG
 *
 * This file is part of ProtonVPN.
 *
 * ProtonVPN is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * ProtonVPN is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with ProtonVPN.  If not, see <https://www.gnu.org/licenses/>.
 */


using NUnit.Framework;
using ProtonVPN.UI.Tests.Results;
using ProtonVPN.UI.Tests.TestsHelper;
using ProtonVPN.UI.Tests.Windows;

namespace ProtonVPN.UI.Tests.Tests
{
    [TestFixture]
    [Category("Connection")]
    public class SecureCoreTests : TestSession
    {
        private readonly LoginWindow _loginWindow = new LoginWindow();
        private readonly HomeWindow _mainWindow = new HomeWindow();
        private readonly SettingsWindow _settingsWindow = new SettingsWindow();
        private readonly HomeResult _homeResult = new HomeResult();

        [Test]
        [Category("Smoke")]
        public void QuickConnectWhileSecureCoreIsEnabled()
        {
            _mainWindow.PressQuickConnectButton()
                .WaitUntilConnected();

            _mainWindow.PressQuickConnectButton()
                .WaitUntilDisconnected();
        }

        [Test]
        public void CheckIfAfterKillingAppSecureCoreConnectionIsRestored()
        {
             _mainWindow.NavigateToSettings();
            _settingsWindow.DisableStartToTray()
                .CloseSettings();
            _mainWindow.PressQuickConnectButton()
                .WaitUntilConnected();
            _homeResult.KillClientAndCheckIfConnectionIsKept();
        }

        [Test]
        public void ConnectAndDisconnectViaMapSecureCore()
        {
            _mainWindow.PerformConnectionViaMapSecureCore(TestConstants.MapCountry)
                .WaitUntilConnected();
            _mainWindow.PerformConnectionViaMapSecureCore(TestConstants.MapCountry)
                .WaitUntilDisconnected();
        }

        [SetUp]
        public void TestInitialize()
        {
            DeleteUserConfig();
            LaunchApp();
            _loginWindow.SignIn(TestUserData.GetPlusUser());
            _mainWindow.EnableSecureCore()
                .CloseSecureCoreWarningModal();
        }

        [TearDown]
        public void TestCleanup()
        {
            Cleanup();
        }
    }
}
