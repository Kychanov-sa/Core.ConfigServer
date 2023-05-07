using GlacialBytes.Core.ConfigServer.WebApi.Server.Services;
using GlacialBytes.Core.ConfigServer.WebApi.Server.Services.Variables;

namespace WebApi.Test.Services.Variables
{
  [TestClass]
  public class VariableServiceTest : BaseServicesTest
  {
    private const string TestProfile = "Test";
    private const string TestVariableName = "Authentication";
    private const string TestVariableValue = "password";
    private IVariableService _variableService;

    static VariableServiceTest()
    {
      VariableService.Configure(_mongoDbConnectionString);
    }

    public VariableServiceTest()
    {
      _variableService = new VariableService();
    }

    [TestInitialize]
    public async Task Initialize()
    {
      await _variableService.SetVariable(TestProfile, TestVariableName, TestVariableValue);
      await _variableService.SetVariable(TestProfile, "IsEnabled", true);
      await _variableService.SetVariable(TestProfile, "Credits", 1124);
    }

    [TestMethod]
    public async Task GetVariable()
    {
      var actualStringValue = await _variableService.GetVariable(TestProfile, TestVariableName);
      Assert.IsNotNull(actualStringValue);
      Assert.AreEqual(TestVariableValue, (string)actualStringValue);

      var actualBoolValue = await _variableService.GetVariable(TestProfile, "IsEnabled");
      Assert.IsNotNull(actualBoolValue);
      Assert.AreEqual(true, (bool)actualBoolValue);

      var actualIntValue = await _variableService.GetVariable(TestProfile, "Credits");
      Assert.IsNotNull(actualIntValue);
      Assert.AreEqual(1124, (int)actualIntValue);
    }

    [TestMethod]
    public async Task GetVariable_WhenNotExists()
    {
      var actualStringValue = await _variableService.GetVariable(TestProfile, "InvalidVariableName");
      Assert.IsNull(actualStringValue);
    }

    [TestMethod]
    public async Task SetVariable()
    {
      string testVarName = "SetVariable_TestVar";
      string testVarValue = Guid.NewGuid().ToString();
       
      await _variableService.SetVariable(TestProfile, testVarName, testVarValue);

      var actualValue = await _variableService.GetVariable(TestProfile, testVarName);
      Assert.AreEqual(testVarValue, actualValue?.ToString());
    }

    [TestMethod]
    public async Task GetVariables()
    {
      var result = await _variableService.GetVariables(TestProfile);
      Assert.IsNotNull(result);
      Assert.IsTrue(result.Any());
      Assert.AreEqual(TestVariableName, result.First().Key);
      Assert.AreEqual(TestVariableValue, result.First().Value?.ToString());
    }

    [TestMethod]
    public async Task GetVariables_WhenProfileNotExists()
    {
      var result = await _variableService.GetVariables("InvalidProfileName");
      Assert.IsNotNull(result);
      Assert.IsFalse(result.Any());
    }
  }
}
