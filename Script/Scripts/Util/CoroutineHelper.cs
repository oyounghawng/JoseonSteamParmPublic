using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions.Comparers;

public static class CoroutineHelper
{
    private static readonly WaitForEndOfFrame WaitForEndOfFrame = new WaitForEndOfFrame();
    private static readonly WaitForFixedUpdate WaitForFixedUpdate = new WaitForFixedUpdate();
    private static readonly Dictionary<float, WaitForSeconds> _waitForSeconds = new Dictionary<float, WaitForSeconds>(new FloatComparer());

    public static WaitForSeconds WaitForSeconds(float seconds)
    {
        WaitForSeconds wfs;

        if (!_waitForSeconds.TryGetValue(seconds, out wfs))
            _waitForSeconds.Add(seconds, wfs = new UnityEngine.WaitForSeconds(seconds));

        return wfs;
    }

    public static Task AsTask(this AsyncOperation asyncOperation)
    {
        var tcs = new TaskCompletionSource<object>();
        asyncOperation.completed += _ => tcs.SetResult(null);
        return tcs.Task;
    }

    public static IEnumerator AwaitTask(Task task)
    {
        while (!task.IsCompleted)
        {
            yield return null;
        }

        if (task.IsFaulted)
        {
            throw task.Exception;
        }
    }

    public static IEnumerator AwaitTask<T>(Task<T> task, System.Action<T> onComplete)
    {
        while (!task.IsCompleted)
        {
            yield return null;
        }

        if (task.IsFaulted)
        {
            throw task.Exception;
        }

        onComplete(task.Result);
    }

    public static IEnumerator AwaitTask<T>(Task<T> task, System.Action<T> onComplete, System.Func<T, bool> additionalCondition)
    {
        T result = default(T);

        while (!task.IsCompleted)
        {
            yield return null;
        }

        if (task.IsFaulted)
        {
            throw task.Exception;
        }

        result = task.Result;
        onComplete(result);

        // 추가 조건이 충족될 때까지 대기
        if (additionalCondition != null)
        {
            yield return new WaitUntil(() => additionalCondition(result));
        }
    }

    public static IEnumerator AwaitCoroutine(IEnumerator coroutine)
    {
        while (coroutine.MoveNext())
        {
            yield return coroutine.Current;
        }
    }
}
