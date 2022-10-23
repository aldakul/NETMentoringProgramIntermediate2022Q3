﻿using MultiThreading.Task3.MatrixMultiplier.Matrices;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task3.MatrixMultiplier.Multipliers
{
    public class MatricesMultiplierParallel : IMatricesMultiplier
    {
        public IMatrix Multiply(IMatrix m1, IMatrix m2)
        {
            var resultMatrix = new Matrix(m1.RowCount, m2.ColCount);
            Parallel.For(0L, m1.RowCount, new ParallelOptions { MaxDegreeOfParallelism = 16 }, (i) =>
            {
                var threadId = Thread.CurrentThread.ManagedThreadId;
                for (var j = 0; j < m2.ColCount; j++)
                {
                    long sum = 0;
                    for (var k = 0; k < m1.ColCount; k++)
                    {
                        sum += m1.GetElement(i, k) * m2.GetElement(k, j);
                    };

                    resultMatrix.SetElement(i, j, sum);
                }
            });

            return resultMatrix;
        }
    }
}
