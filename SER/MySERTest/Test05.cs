using System.Collections.Generic;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace MySER;

public class Test05
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
  public void test05() {
    driver.Navigate().GoToUrl("https://localhost:7173/");
    driver.FindElement(By.Id("inputUsuario")).SendKeys("josuecapi");
    driver.FindElement(By.Id("inputContraseña")).SendKeys("1234");
    driver.FindElement(By.Id("btnIniciarSesion")).Click();
    driver.FindElement(By.Id("btnExperienciasEducativas")).Click();
    driver.FindElement(By.Id("btnRegistrarExp")).Click();
    driver.FindElement(By.Id("nombreList")).Click();
    {
      var dropdown = driver.FindElement(By.Id("nombreList"));
      dropdown.FindElement(By.XPath("//option[. = 'Experiencia recepcional']")).Click();
    }
    driver.FindElement(By.CssSelector("#nombreList > option:nth-child(2)")).Click();
    driver.FindElement(By.Id("inputNRC")).Click();
    driver.FindElement(By.Id("inputNRC")).SendKeys("3145");
    driver.FindElement(By.Id("seccionList")).Click();
    {
      var dropdown = driver.FindElement(By.Id("seccionList"));
      dropdown.FindElement(By.XPath("//option[. = 'AGO22 - FEB23']")).Click();
    }
    
    driver.FindElement(By.CssSelector(".d-flex:nth-child(3) option:nth-child(2)")).Click();
    driver.FindElement(By.Name("ExperienciaEducativa.Seccion")).Click();
    {
      var dropdown = driver.FindElement(By.Name("ExperienciaEducativa.Seccion"));
      dropdown.FindElement(By.XPath("//option[. = '2']")).Click();
    }
    driver.FindElement(By.Id("btnGuardarExp")).Click();
    driver.FindElement(By.LinkText("Experiencias educativas")).Click();
    driver.FindElement(By.Id("formFile")).Click();
    driver.FindElement(By.Id("formFile")).SendKeys("C:\\fakepath\\CU02_RegistrarDocumentoDeProyectoGuiado_Prototipos.pdf");
    driver.FindElement(By.Id("btnGuardarExp")).Click();
    driver.Close();
  }
}