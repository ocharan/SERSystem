using System.Collections.Generic;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace MySER;

public class Test31
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
    public void test31()
    {
        driver.Navigate().GoToUrl("https://localhost:7173/");
        driver.Manage().Window.Size = new System.Drawing.Size(1846, 1053);
        driver.FindElement(By.Id("inputUsuario")).SendKeys("josuecg");
        driver.FindElement(By.Id("inputContraseña")).SendKeys("1234");
        driver.FindElement(By.Id("btnIniciarSesion")).Click();
        driver.FindElement(By.Id("btnCuerpoAcademico")).Click();
        driver.FindElement(By.Id("btnProyectosInv")).Click();
        driver.FindElement(By.Id("btnRegistrarProyecto")).Click();
        
        driver.FindElement(By.Id("select_tipo")).Click();
        {
            var dropdown = driver.FindElement(By.Id("select_tipo"));
            dropdown.FindElement(By.XPath("//option[. = 'Proyecto de investigación']")).Click();
        }
        
        driver.FindElement(By.Id("btn_Confirmar")).Click();
        
        driver.FindElement(By.Id("input_nombreLgac")).SendKeys("Desarrollo de inteligencia artificial aplicada y metodos logaritmicos");
        driver.FindElement(By.Id("datepicker")).Click();
        driver.FindElement(By.Id("datepicker")).SendKeys("2018-02-20");
        driver.FindElement(By.Id("cuerpos_list")).Click();
        {
            var dropdown = driver.FindElement(By.Id("cuerpos_list"));
            dropdown.FindElement(By.XPath("//option[. = 'Cuerpo académico de ingeniería de software y tecnologias']")).Click();
        }
        driver.FindElement(By.Id("btnGuardarPI")).Click();
    }
}