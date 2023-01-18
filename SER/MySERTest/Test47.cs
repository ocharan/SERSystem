using System.Collections.Generic;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace MySER;

public class Test47
{ 
    private IWebDriver driver;
    public IDictionary<string, object> vars {get; private set;}
    private IJavaScriptExecutor js;
    [SetUp]
    public void SetUp() {
        FirefoxOptions firefoxOptions = new FirefoxOptions();
        firefoxOptions.AcceptInsecureCertificates = true;
        driver = new FirefoxDriver(firefoxOptions);
        js = (IJavaScriptExecutor)driver;
        vars = new Dictionary<string, object>();
    }
    [TearDown]
    protected void TearDown() {
        driver.Quit();
    }

    [Test]
    public void test47()
    {
        driver.Navigate().GoToUrl("https://localhost:7173/");
        driver.Manage().Window.Size = new System.Drawing.Size(1846, 1053);
        driver.FindElement(By.Id("inputUsuario")).SendKeys("josuecg");
        driver.FindElement(By.Id("inputContraseña")).SendKeys("1234");
        driver.FindElement(By.Id("btnIniciarSesion")).Click();
        driver.FindElement(By.Id("btnExperienciaRecepcional")).Click();
        driver.FindElement(By.Id("btnSinodales")).Click();
        driver.FindElement(By.Id("btnModificarInterno&2016")).Click();
        driver.FindElement(By.Id("inputCorreo")).Clear();
        driver.FindElement(By.Id("inputCorreo")).SendKeys("diegofersanvalle@gmail.com");
        driver.FindElement(By.Id("inputTelefono")).Clear();
        driver.FindElement(By.Id("inputTelefono")).SendKeys("2266995547");

        
        driver.FindElement(By.Id("btnGuardarSinodal")).Click();
    }
}