using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IResidenceRepository Residence{ get; }
        IReservationRepository Reservation { get; }
        ILandlordReviewsRepository LandlordReviews { get; }
        IResidenceReviewsRepository ResidenceReviews { get; }
        IImageRepository Image { get; }
        void Save();
    }
}
