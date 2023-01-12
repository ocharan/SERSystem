using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
namespace SERTest
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                FirefoxOptions firefoxOptions = new FirefoxOptions();
                firefoxOptions.AcceptInsecureCertificates = true;
                WebDriver driver = new FirefoxDriver(firefoxOptions);
                Test01 test01 = new Test01();
                test01.StartTest(driver);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}