using FileUtility;
using FileUtility.Commands;
using FileUtility.Models;

namespace UnitTest
{
    public class Tests
    {
        string programBaseDir;
        string tempDir;
        string targetDir;

        [SetUp]
        public void Setup()
        {
            programBaseDir = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, System.AppDomain.CurrentDomain.RelativeSearchPath ?? "");
            tempDir = Path.Combine(programBaseDir, "temp");
            targetDir = Path.Combine(programBaseDir, "target");

            //clear tempDir and targetDir 
            if (Directory.Exists(tempDir))
                Directory.Delete(tempDir, true);

            if (Directory.Exists(targetDir))
                Directory.Delete(targetDir, true);
        }

        [Test]
        public void EnumerateFiles_Test()
        {
            string sourceDirectory = Path.Combine(programBaseDir, "testfiles");
            string[] filters = ["test*.txt"];

            FileManager fileManager = new FileManager();

            var files = fileManager.EnumerateFiles(sourceDirectory, filters);

            Assert.IsTrue(files.Count == 15);
        }


        [Test]
        public void FileManager_Test()
        {


            string sourceDirectory = Path.Combine(programBaseDir, "testfiles");
            string[] filters = ["test*.txt"];

            FileManager fileManager = new FileManager();

            var files = fileManager.EnumerateFiles(sourceDirectory, filters);

            try
            {

                Parallel.ForEach(files,
                    new ParallelOptions { MaxDegreeOfParallelism = 1 },
                    file =>
                    {
                        var commandManager = new FileCommandManager();

                        commandManager.AddCommand(new CopyToTempDirectoryCommand(file, tempDir));
                        commandManager.AddCommand(new CopyToTargetDirectoryCommand(file, targetDir, true));

                        //add new commands here...

                        commandManager.ProcessCommands();
                    });
            }
            catch (AggregateException ex) { }
            catch (Exception ex) { }
            finally { }

            //check that the targetDir should have the correct number of files
            int targetFileCount = Directory.GetFiles(targetDir, "*", SearchOption.AllDirectories).Length;
            Assert.AreEqual(15, targetFileCount);
        }
    }
}