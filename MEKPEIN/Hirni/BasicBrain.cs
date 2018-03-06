using BrightWire;
using BrightWire.ExecutionGraph;
using System;
using System.Collections.Generic;
using System.Text;

namespace MEKPEIN.Hirni
{
    public class BasicBrain
    {
        public static void CrazyTest()
        {
            using (var la = BrightWireProvider.CreateLinearAlgebra())
            {
                var graph = new GraphFactory(la);

                graph.CurrentPropertySet
                    .Use(graph.GradientDescent.Adam)
                    .Use(graph.GaussianWeightInitialisation(false, 0.1f, GaussianVarianceCalibration.SquareRoot2N));

                var dtbuilder = BrightWireProvider.CreateDataTableBuilder();
                dtbuilder.AddColumn(ColumnType.Tensor, "image", false);
                dtbuilder.AddColumn(ColumnType.Vector, "expectedoutput", true);

                // DATEN GENERIEREN
                for (int i = 0; i < 666; i++)
                {
                    var fv = new BrightWire.Models.FloatVector
                    {
                        Data = new float[666],
                    };

                    var fv2 = new BrightWire.Models.FloatVector
                    {
                        Data = new float[666],
                    };

                    dtbuilder.Add(fv, fv2);
                }
                var datatable = dtbuilder.Build();

                Console.WriteLine($"####### TRAINING START #######");

                // create the engine
                var (Training, Test) = datatable.Split();
                var trainingData = graph.CreateDataSource(Training);

                var testData = graph.CreateDataSource(Test);
                var engine = graph.CreateTrainingEngine(trainingData, learningRate: 0.01f, batchSize: 16);

                // build the network
                var errorMetric = graph.ErrorMetric.Quadratic;

                const int HIDDEN_LAYER_SIZE = 2000, TRAINING_ITERATIONS = 2000;

                var network = graph.Connect(engine)
                    .AddFeedForward(HIDDEN_LAYER_SIZE)
                    .Add(graph.ReluActivation())
                    //.AddBackpropagation(errorMetric)
                    .AddFeedForward(HIDDEN_LAYER_SIZE)
                    .Add(graph.ReluActivation())
                    //.AddBackpropagation(errorMetric)
                    .AddDropOut(.2f)
                    .AddFeedForward(engine.DataSource.OutputSize)
                    //.Add(graph.ReluActivation())
                    .AddBackpropagation(errorMetric)
                //.AddBackpropagationThroughTime(errorMetric)
                ;

                // train the network for twenty iterations, saving the model on each improvement
                BrightWire.Models.ExecutionGraph bestGraph = null;
                engine.Train(TRAINING_ITERATIONS, testData, errorMetric, ((modelGraph) => bestGraph = modelGraph.Graph));

                // export the graph and verify it against some unseen integers on the best model
                var executionEngine = graph.CreateEngine(bestGraph ?? engine.Graph);
            }
        }
    }
}
