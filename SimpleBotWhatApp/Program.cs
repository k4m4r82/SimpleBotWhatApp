/**
 * Copyright (C) 2018 Kamarudin (http://coding4ever.net/)
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not
 * use this file except in compliance with the License. You may obtain a copy of
 * the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the
 * License for the specific language governing permissions and limitations under
 * the License.
 *
 * The latest version of this file can be found at https://github.com/k4m4r82/SimpleBotWhatApp
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Threading;

namespace SimpleBotWhatApp
{
    class Program
    {        
        private const string QR_CODE_XPATH = "//img[@alt='Scan me!']";
        private const string SEARCH_INPUT_XPATH = "//input[@title='Search or start new chat']";
        private const string CHAT_INPUT_XPATH = "//div[@class='_2S1VP copyable-text selectable-text']";
        private static int TUNDA = 2000;

        private static IWebDriver _driver = null;

        static void Main(string[] args)
        {
            Console.Title = "Simple Bot WhatApp";    
        
            StartWhatApp();
            Console.Clear();

            while (OnLoginPage())
            {
                Console.WriteLine("Silahkan login terlebih dulu ...");
                Thread.Sleep(TUNDA);
            }            

            Console.Clear();
            Console.Write("Nomor WA Target (pake +62): ");
            var nomorWA = Console.ReadLine();

            Console.Write("Pesan yang mau dikirim: ");
            var pesan = Console.ReadLine();

            Console.Write("Jumlah pesan yang dikirim (jangan kebanyakan loh ya hehe): ");
            var jumlah = Convert.ToInt32(Console.ReadLine());
            if (jumlah == 0) jumlah = 1;

            var txtCariNoWa = _driver.FindElement(By.XPath(SEARCH_INPUT_XPATH));
            txtCariNoWa.SendKeys(nomorWA);
            txtCariNoWa.SendKeys(Keys.Enter);

            var txtPesan = _driver.FindElement(By.XPath(CHAT_INPUT_XPATH));

            Console.WriteLine();
            for (int i = 0; i < jumlah; i++)
            {
                txtPesan.SendKeys(pesan);
                txtPesan.SendKeys(Keys.Enter);

                Console.WriteLine("Pesan '{0}' sudah dikirim", pesan);
                Thread.Sleep(TUNDA);
            }

            Console.WriteLine("\nPress any key to exit ...");
            Console.ReadKey();
            StopWhatApp();

        }        

        static void StartWhatApp()
        {
            _driver = new ChromeDriver(new ChromeOptions { LeaveBrowserRunning = false });
            _driver.Url = "https://web.whatsapp.com";

            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);
            _driver.Navigate();
        }

        static bool OnLoginPage()
        {
            try
            {
                if (_driver.FindElement(By.XPath(QR_CODE_XPATH)) != null)
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
            return false;
        }

        static void StopWhatApp()
        {
            _driver.Quit();
            _driver.Dispose();
        }
    }
}
