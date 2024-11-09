using MarriageAgency.ViewModels;

namespace MarriageAgency.Services
{
    public interface IServiceService
    {
        HomeViewModel GetHomeViewModel(int numberRows = 10);
    }
}
