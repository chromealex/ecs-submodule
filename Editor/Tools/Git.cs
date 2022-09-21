namespace ME.ECSEditor {

    using System;
    using UnityEngine;
    using System.Diagnostics;
    using System.Text;

    /// <summary>
    /// GitException includes the error output from a Git.Run() command as well as the
    /// ExitCode it returned.
    /// </summary>
    public class GitException : InvalidOperationException {

        public GitException(int exitCode, string errors) : base(errors) {
            this.ExitCode = exitCode;
        }

        /// <summary>
        /// The exit code returned when running the Git command.
        /// </summary>
        public readonly int ExitCode;

    }

    public static class Git {

        /* Properties ============================================================================================================= */

        /// <summary>
        /// Retrieves the build version from git based on the most recent matching tag and
        /// commit history. This returns the version as: {major.minor.build} where 'build'
        /// represents the nth commit after the tagged commit.
        /// Note: The initial 'v' and the commit hash code are removed.
        /// </summary>
        public static string BuildVersion {
            get {
                var version = Git.Run(@"describe --tags --long --match ""v[0-9]*""");
                // Remove initial 'v' and ending git commit hash.
                version = version.Replace('-', '.');
                version = version.Substring(1, version.LastIndexOf('.') - 1);
                return version;
            }
        }

        public static string RootDir => Git.Run("rev-parse --show-toplevel");
        
        /// <summary>
        /// The currently active branch.
        /// </summary>
        public static string Branch => Git.Run(@"rev-parse --abbrev-ref HEAD");

        /// <summary>
        /// Returns a listing of all uncommitted or untracked (added) files.
        /// </summary>
        public static string Status => Git.Run(@"status --porcelain");

        /* Methods ================================================================================================================ */

        /// <summary>
        /// Runs git.exe with the specified arguments and returns the output.
        /// </summary>
        public static string Run(string arguments) {
            
            return Run(arguments, Application.dataPath);

        }

        public static string Run(string arguments, string path) {
            using (var process = new System.Diagnostics.Process()) {
                var exitCode = process.Run(@"git", arguments, path, out var output, out var errors);
                if (exitCode == 0) {
                    return output;
                } else {
                    throw new GitException(exitCode, errors);
                }
            }
        }

    }

    public static class ProcessExtensions {

        /* Properties ============================================================================================================= */

        /* Methods ================================================================================================================ */

        /// <summary>
        /// Runs the specified process and waits for it to exit. Its output and errors are
        /// returned as well as the exit code from the process.
        /// See: https://stackoverflow.com/questions/4291912/process-start-how-to-get-the-output
        /// Note that if any deadlocks occur, read the above thread (cubrman's response).
        /// </summary>
        /// <remarks>
        /// This should be run from a using block and disposed after use. It won't 
        /// work properly to keep it around.
        /// </remarks>
        public static int Run(this Process process, string application, string arguments, string workingDirectory, out string output, out string errors) {
            process.StartInfo = new ProcessStartInfo {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                FileName = application,
                Arguments = arguments,
                WorkingDirectory = workingDirectory,
            };

            // Use the following event to read both output and errors output.
            var outputBuilder = new StringBuilder();
            var errorsBuilder = new StringBuilder();
            process.OutputDataReceived += (_, args) => outputBuilder.AppendLine(args.Data);
            process.ErrorDataReceived += (_, args) => errorsBuilder.AppendLine(args.Data);

            // Start the process and wait for it to exit.
            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();

            output = outputBuilder.ToString().TrimEnd();
            errors = errorsBuilder.ToString().TrimEnd();
            return process.ExitCode;
        }

    }

}