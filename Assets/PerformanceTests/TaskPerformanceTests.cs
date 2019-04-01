using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniRx.Async;
using Unity.PerformanceTesting;
using UnityEngine;
using UnityEngine.TestTools;

public class TaskPerformanceTests
{
    [PerformanceUnityTest]
    public IEnumerator TaskCompletionSourceTest()
    {
        yield return new EnterPlayMode();
        Measure.Method(() =>
            {
                for (int i = 0; i < 100; i++)
                {
                    TestUniTask();
                }

            })
            .WarmupCount(10)
            .MeasurementCount(10)
            .IterationsPerMeasurement(5)
            .GC()
            .Definition(new SampleGroupDefinition("UniTaskCompletionSource"))
            .Run();

        Measure.Method(() =>
            {
                for (int i = 0; i < 100; i++)
                {
                    TestTask();
                }
            })
            .WarmupCount(10)
            .MeasurementCount(10)
            .IterationsPerMeasurement(5)
            .GC()
            .Definition(new SampleGroupDefinition("TaskCompletionSource"))
            .Run();

        yield return new ExitPlayMode();
    }

    Task TestTask()
    {
        return new TaskCompletionSource<bool>().Task;
    }

    UniTask TestUniTask()
    {
        return new UniTaskCompletionSource<bool>().Task;
    }

    [PerformanceUnityTest]
    public IEnumerator AsyncMethodTest()
    {
        yield return new EnterPlayMode();

        Measure.Method(() =>
            {
                for (int i = 0; i < 100; i++)
                {
                    AsyncUniTaskTest();
                }
            })
            .WarmupCount(10)
            .MeasurementCount(10)
            .IterationsPerMeasurement(5)
            .GC()
            .Definition(new SampleGroupDefinition("UniTaskAsyncMethod"))
            .Run();

        Measure.Method(() =>
            {
                for (int i = 0; i < 100; i++)
                {
                    AsyncTaskTest();
                }
            })
            .WarmupCount(10)
            .MeasurementCount(10)
            .IterationsPerMeasurement(5)
            .GC()
            .Definition(new SampleGroupDefinition("TaskAsyncMethod"))
            .Run();

        yield return new ExitPlayMode();
    }

    async UniTask AsyncUniTaskTest()
    {
        await UniTask.Delay(5);
        await UniTask.Delay(5);
        await UniTask.WhenAll(
            UniTask.Delay(5),
            UniTask.Delay(5),
            UniTask.Delay(5));

        await UniTask.Delay(5);
    }

    async Task AsyncTaskTest()
    {
        await Task.Delay(5);
        await Task.Delay(5);
        await Task.WhenAll(
            Task.Delay(5),
            Task.Delay(5),
            Task.Delay(5));

        await Task.Delay(5);
    }
}
