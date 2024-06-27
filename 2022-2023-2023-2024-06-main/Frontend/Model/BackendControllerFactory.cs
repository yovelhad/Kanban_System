using IntroSE.Kanban.Backend.ServiceLayer;
using System;

namespace Frontend.Model;

public static class BackendControllerFactory
{
    private static Services? services;

    /// <summary>
    /// Creates an instance of the system loading all the data from the DB.
    /// </summary>
    /// <returns>Services object that will contain the entire data of the system.</returns>
    public static Services GetFactory()
    {
        if (services == null)
        {
            ServiceFactory sf = new ServiceFactory();
            services = sf.create();
            services.USER_SERVICE.LoadUsers();
            services.BOARD_SERVICE.LoadBoards();
            Console.WriteLine("all loaded");
        }
        return services;
    }
}