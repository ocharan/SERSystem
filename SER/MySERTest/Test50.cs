using System.Collections.Generic;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace MySER;

public class Test50
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
    public void test50()
    {
        driver.Navigate().GoToUrl("https://localhost:7173/");
        driver.Manage().Window.Size = new System.Drawing.Size(1846, 1053);
        driver.FindElement(By.Id("inputUsuario")).SendKeys("josuecg");
        driver.FindElement(By.Id("inputContraseña")).SendKeys("1234");
        driver.FindElement(By.Id("btnIniciarSesion")).Click();
        driver.FindElement(By.Id("btnExperienciaRecepcional")).Click();
        driver.FindElement(By.Id("btnTrabajos")).Click();
        driver.FindElement(By.Id("btnRegistrarTrabajo")).Click();
        
        driver.FindElement(By.Id("TrabajoRecepcional_Nombre")).SendKeys("Diseño de algoritmos evolutivos");
        
        driver.FindElement(By.Id("TrabajoRecepcional_Duracion")).Click();
        {
            var dropdown = driver.FindElement(By.Id("TrabajoRecepcional_Duracion"));
            dropdown.FindElement(By.XPath("//option[. = '12 meses']")).Click();
        }
        
        driver.FindElement(By.Id("datepicker")).Click();
        driver.FindElement(By.Id("datepicker")).SendKeys("2023-02-02");
        driver.FindElement(By.Id("TrabajoRecepcional_LineaDeInvestigacion")).SendKeys("Inteligencia artificial");

        driver.FindElement(By.Id("TrabajoRecepcional_Modalidad")).Click();
        {
            var dropdown = driver.FindElement(By.Id("TrabajoRecepcional_Modalidad"));
            dropdown.FindElement(By.XPath("//option[. = 'Tesina']")).Click();
        }

        driver.FindElement(By.Id("selectTipo")).Click();
        {
            var dropdown = driver.FindElement(By.Id("selectTipo"));
            dropdown.FindElement(By.XPath("//option[. = 'Vinculación']")).Click();
        }
        
        driver.FindElement(By.Id("estadoExperiencia")).Click();
        {
            var dropdown = driver.FindElement(By.Id("selectTipo"));
            dropdown.FindElement(By.XPath("//option[. = 'Proyecto guiado']")).Click();
        }
        
        driver.FindElement(By.Id("selectProyecto")).Click();
        {
            var dropdown = driver.FindElement(By.Id("selectProyecto"));
            dropdown.FindElement(By.XPath("//option[. = 'Instituto de inteligencia artificial y fisica']")).Click();
        }
        
        
        
        driver.FindElement(By.Id("btnGuardarProyecto")).Click();

        string mensajeRecibido = driver.FindElement(By.Id("alertError")).Text;
        string mensajeEsperado = "El trabajo recepcional que intentas registrar ya existe";
        Assert.AreEqual(mensajeEsperado, mensajeRecibido);
    }
}