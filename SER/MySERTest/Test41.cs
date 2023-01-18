using System.Collections.Generic;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace MySER;

public class Test41
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
    public void test41()
    {
        driver.Navigate().GoToUrl("https://localhost:7173/");
        driver.Manage().Window.Size = new System.Drawing.Size(1846, 1053);
        driver.FindElement(By.Id("inputUsuario")).SendKeys("josuecg");
        driver.FindElement(By.Id("inputContraseña")).SendKeys("1234");
        driver.FindElement(By.Id("btnIniciarSesion")).Click();
        driver.FindElement(By.Id("btnCuerpoAcademico")).Click();
        driver.FindElement(By.Id("btnOrganizaciones")).Click();

        driver.FindElement(By.Id("btnRegistrarOrg")).Click();

        driver.FindElement(By.Id("inputNombreOrganizacion")).SendKeys("Instituto de inteligencia artificial");

        driver.FindElement(By.Id("btnGuardarOrganizacion")).Click();

        string mensajeRecibido = driver.FindElement(By.Id("alertError")).Text;
        string mensajeEsperado = "La organización que intentas registrar con ese nombre, ya existe";
        
        Assert.AreEqual(mensajeEsperado, mensajeRecibido);
    }
}