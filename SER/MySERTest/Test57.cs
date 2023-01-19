using System.Collections.Generic;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace MySER;

public class Test57
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
    public void test56()
    {
        driver.Navigate().GoToUrl("https://localhost:7173/");
        driver.Manage().Window.Size = new System.Drawing.Size(1846, 1053);
        driver.FindElement(By.Id("inputUsuario")).SendKeys("josuecg");
        driver.FindElement(By.Id("inputContraseña")).SendKeys("1234");
        driver.FindElement(By.Id("btnIniciarSesion")).Click();
        driver.FindElement(By.Id("btnExperienciaRecepcional")).Click();
        driver.FindElement(By.Id("btnTrabajos")).Click();
        driver.FindElement(By.Id("btnAsignarSinodales&2017")).Click();
        
        driver.FindElement(By.Id("select_tipo")).Click();
        {
            var dropdown = driver.FindElement(By.Id("select_tipo"));
            dropdown.FindElement(By.XPath("//option[. = 'Director']")).Click();
        }
        
        driver.FindElement(By.Id("btnAsignar2016")).Click();
        driver.FindElement(By.Id("btnAsignar2017")).Click();
        

        string mensajeRecibido = driver.FindElement(By.Id("errorDirector")).Text;
        string mensajeEsperado = "Ya hay un director registrado en este trabajo recepcional.";
        
        Assert.AreEqual(mensajeEsperado, mensajeRecibido);

    }
}