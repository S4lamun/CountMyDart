﻿namespace CountMyDartMaui
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(InteractiveDartboard), typeof(InteractiveDartboard));
        }
    }
}
