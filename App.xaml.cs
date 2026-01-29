using System.Configuration;
using System.Data;
using System.Windows;

namespace HexMinesweeper
{
    /// <summary>
    /// ==================== App.xaml.cs ====================
    /// 
    /// APPLICATION ENTRY POINT
    /// 
    /// PURPOSE:
    /// This is the application entry point for the HexMinesweeper WPF desktop application.
    /// The App class inherits from WPF's Application class and serves as the bootstrap
    /// for the entire application lifecycle.
    /// 
    /// HOW IT WORKS:
    /// When you run the HexMinesweeper.exe, Windows launches the application and the CLR
    /// (Common Language Runtime) automatically instantiates this App class and calls its
    /// constructor. The code-behind file (this file) is merged with App.xaml (which defines
    /// the StartupUri pointing to MainWindow) to create a complete application instance.
    /// 
    /// KEY RESPONSIBILITIES:
    /// 1. Application Initialization: The App class is responsible for setting up the
    ///    application context when the program starts.
    /// 2. Lifetime Management: WPF automatically manages the application's lifecycle through
    ///    the Application base class, including startup, shutdown, and event handling.
    /// 3. Resource Sharing: While not used in this project, App can define global resources
    ///    (like brushes, styles, or templates) that are available to all windows and controls.
    /// 4. Global Event Handling: The Application class provides events like Startup, Exit,
    ///    and Exceptions that can be handled here for global application behavior.
    /// 
    /// WPF CONCEPTS FOR BEGINNERS:
    /// - XAML (App.xaml): This is the markup file that declares the application structure
    ///   and specifies which window appears first (StartupUri="MainWindow.xaml").
    /// - Code-Behind (this file): Contains C# logic for the XAML file.
    /// - Partial Class: The App class is marked as 'partial' because Visual Studio splits
    ///   application definition between App.xaml (designer-generated code) and App.xaml.cs
    ///   (your custom code). Both parts are combined during compilation.
    /// - Inheritance: By inheriting from Application, this class gains automatic access to
    ///   all WPF application functionality without needing to manually code it.
    /// 
    /// EXEMPLAR NOTE:
    /// This App class is intentionally simple. It demonstrates the minimal setup needed
    /// for a WPF application. In more complex projects, you might override methods like
    /// OnStartup() or OnExit() to perform custom initialization or cleanup operations.
    /// 
    /// ========================================================
    /// </summary>
    public partial class App : Application
    {
        // This class body is intentionally empty because all necessary functionality
        // is inherited from the Application base class. The actual application startup
        // is configured in App.xaml via the StartupUri attribute.
    }

}
