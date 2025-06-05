namespace UnitTests.Tests
{
    public static class DumbestFunctionTest
    {
        //Naming Convention  - ClassName_MethodName_ExpectedResult

        //Arrange - Act - Assert

        public static void DumbestFunction_ReturnsPikachuIfZero_ReturnsString()
        {
            try
            {
                //Arrange
                int num = 0;
                DumbestFunction func = new DumbestFunction();
                string result = null;

                //Act
                result = func.ReturnsPikachuIfZero(num);

                //Assert
                if (result == "Pikachu")
                {
                    Console.WriteLine("PASSED: DumbestFunction.ReturnsPikachuIfZero_ReturnsString");
                }
                else
                {
                    Console.WriteLine("FAILED: DumbestFunction.ReturnsPikachuIfZero_ReturnsString");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
