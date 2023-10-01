using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.DataAccess.Repository.IRepository;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using WebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace WebApp.Services
{
    public class MatrixFactorization
    {
        private readonly IServiceProvider _serviceProvider;
        private Matrix<double>? ResidenceDataMatrix;
        private Matrix<double>? ResidenceRecommendationsMatrix;
        public MatrixFactorization(IServiceProvider service)
        {
            _serviceProvider = service;
            //ResidenceRecommendationsMatrix = [];
        }

        public List<Residence> ResidenceRecomendations(string userId)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                var _unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                List<User> users = _userManager.Users.ToList();
                List<Residence> residences = _unitOfWork.Residence.GetAll().ToList();
                int index = 0;
                foreach (var user in users)
                {
                    if (user.Id == user.Id)
                        break;
                    index++;
                }
                Console.WriteLine("POSITION OF THE USER IS " + index);
                Dictionary<int, double> ratings = new Dictionary<int, double>();

                for (int i = 0; i < ResidenceRecommendationsMatrix.ColumnCount; i++)
                {
                    if (ResidenceRecommendationsMatrix[index, i] != 0)
                    {
                        ratings[i] = ResidenceRecommendationsMatrix[index, i];
                    }
                }
                var sortedRatings = ratings.OrderByDescending(pair => pair.Value);

                List<Residence> idsOrdered = sortedRatings.Select(pair => residences[pair.Key]).ToList();

                Console.WriteLine(string.Join(", ", idsOrdered));
                return idsOrdered;
            }
        }



        public void ResidenceMatrixFactorization()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                var _userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                List<User> users = _userManager.Users.ToList();
                List<Residence> residences = _unitOfWork.Residence.GetAll().ToList();
                ResidenceDataMatrix = Matrix<double>.Build.Dense(users.Count, residences.Count);
                int userIndex = 0;
                foreach (var user in users)
                {
                    List<ViewedResidences> viewedResidences = _unitOfWork.ViewedResidences.GetAll(x => x.UserId == user.Id).ToList();
                    List<Reservation> userReservations = _unitOfWork.Reservation.GetAll(x => x.StayingMemberUsername == user.UserName).ToList();
                    List<SearchedNeighborhoods> searchedNeighborhoods = _unitOfWork.SearchedNeighborhoods.GetAll(x => x.UserId == user.Id).ToList();

                    foreach (var viewedResidence in viewedResidences)
                    {
                        int residenceIndex = residences.FindIndex(x => x.Id == viewedResidence.ResidenceId);
                        double newValue = ResidenceDataMatrix[userIndex, residenceIndex] + 1;
                        ResidenceDataMatrix[userIndex, residenceIndex] = newValue;
                    }

                    foreach (var userReservation in userReservations)
                    {
                        int residenceIndex = residences.FindIndex(x => x.Id == userReservation.ResidenceId);
                        double newValue = ResidenceDataMatrix[userIndex, residenceIndex] + 1;
                        ResidenceDataMatrix[userIndex, residenceIndex] = newValue;
                    }

                    foreach (var searchedNeighborhood in searchedNeighborhoods)
                    {
                        var residencesWithNeighborhood = _unitOfWork.Residence.GetAll(x => x.Neighborhood == searchedNeighborhood.Neighborhood).ToList();
                        foreach (var residence in residencesWithNeighborhood)
                        {
                            int residenceIndex = residences.FindIndex(x => x.Id == residence.Id);
                            double newValue = ResidenceDataMatrix[userIndex, residenceIndex] + 1;
                            ResidenceDataMatrix[userIndex, residenceIndex] = newValue;
                        }
                    }

                    userIndex++;
                }

                int k = 3;
                List<double> list_of_h = new List<double> { 0.01, 0.001, 0.0001, 0.00001 };
                double min_error = 999999;
                double best_h = 0.01;
                var rand = new System.Random(); // Random number generator
                Matrix<double> best_matrix = null;

                foreach (double h in list_of_h)
                {
                    var random = new Random();
                    var V = Matrix<double>.Build.Dense(ResidenceDataMatrix.RowCount, k, (i, j) => random.NextDouble() * 4 + 1);
                    var F = Matrix<double>.Build.Dense(k, ResidenceDataMatrix.ColumnCount, (i, j) => random.NextDouble() * 4 + 1);

                    var tuple = Algorithm(ResidenceDataMatrix, V, F, k, h);
                    var error = tuple.Item2;

                    if (error < min_error)
                    {
                        min_error = error;
                        best_h = h;
                        best_matrix = tuple.Item1;
                    }
                }

                ResidenceRecommendationsMatrix = best_matrix;
            }
        }

        private Tuple<Matrix<double>, double> Algorithm(Matrix<double> dataMatrix, Matrix<double> V, Matrix<double> F, int k, double h)
        {
            int max_iters = 1000;
            double err = 999999;
            double prev_err;
            double x_;

            for (int iter = 0; iter <= max_iters; iter++)
            {
                for (int i = 0; i < dataMatrix.RowCount; i++)
                {
                    for (int j = 0; j < dataMatrix.ColumnCount; j++)
                    {
                        if (dataMatrix[i, j] > 0)
                        {
                            x_ = 0;

                            for (int n = 0; n < k; n++)
                            {
                                x_ += V[i, n] * F[n, j];
                            }

                            double e = dataMatrix[i, j] - x_;

                            for (int n = 0; n < k; n++)
                            {
                                V[i, n] = V[i, n] + h * 2 * e * F[n, j];
                                F[n, j] = F[n, j] + h * 2 * e * V[i, n];
                            }
                        }
                    }
                }

                prev_err = err;
                err = 0;

                for (int i = 0; i < dataMatrix.RowCount; i++)
                {
                    for (int j = 0; j < dataMatrix.ColumnCount; j++)
                    {
                        if (dataMatrix[i, j] > 0)
                        {
                            x_ = 0;

                            for (int n = 0; n < k; n++)
                            {
                                x_ += V[i, n] * F[n, j];
                            }

                            err += Math.Pow(dataMatrix[i, j] - x_, 2);
                        }
                    }
                }

                if (prev_err <= err)
                {
                    Console.WriteLine("Iter: " + iter);
                    break;
                }
            }

            Matrix<double> recommendationsMatrix = V.Multiply(F);
            return Tuple.Create(recommendationsMatrix, err);
        }

    }
}
