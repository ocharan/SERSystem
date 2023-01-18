using System.Collections.Generic;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace MySER;

public class Test44
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
    public void test44()
    {
        driver.Navigate().GoToUrl("https://localhost:7173/");
        driver.Manage().Window.Size = new System.Drawing.Size(1846, 1053);
        driver.FindElement(By.Id("inputUsuario")).SendKeys("josuecg");
        driver.FindElement(By.Id("inputContraseña")).SendKeys("1234");
        driver.FindElement(By.Id("btnIniciarSesion")).Click();
        driver.FindElement(By.Id("btnExperienciaRecepcional")).Click();
        driver.FindElement(By.Id("btnSinodales")).Click();
        driver.FindElement(By.Id("btnRegistrarSinodal")).Click();
        
        driver.FindElement(By.Id("select_tipo")).Click();
        {
            var dropdown = driver.FindElement(By.Id("select_tipo"));
            dropdown.FindElement(By.XPath("//option[. = 'Sinodal interno']")).Click();
        }
        
        driver.FindElement(By.Id("btnConfirmar")).Click();

        
        driver.FindElement(By.Id("btnSeleccionar$12345")).Click();
        
        driver.FindElement(By.Id("cuerpos_list")).Click();
        {
            var dropdown = driver.FindElement(By.Id("cuerpos_list"));
            dropdown.FindElement(By.XPath("//option[. = 'Facultad de estádistica e informática']")).Click();
        }
        driver.FindElement(By.Id("inputCorreo")).SendKeys("diegofersan@gmail.com");
        driver.FindElement(By.Id("inputTelefono")).SendKeys("2266995544");

        
        driver.FindElement(By.Id("btnGuardarSinodal")).Click();
        
        
        string mensajeRecibido = driver.FindElement(By.Id("alertError")).Text;
        string mensajeEsperado = "El profesor seleccionado ya se encuentra registrado como sinodal";
        
        Assert.AreEqual(mensajeEsperado, mensajeRecibido);
    }
}