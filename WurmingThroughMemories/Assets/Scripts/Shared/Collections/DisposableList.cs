using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine.PlayerLoop;

  public abstract class DisposableList<T> : List<T>, IDisposable {

    protected abstract void OnPreClear();

    public new void Clear(){
      OnPreClear();
      base.Clear();
    }

    public void Dispose() {
      Clear();
    }
  }

  public class DisposableList : DisposableList<IDisposable> {
	  protected override void OnPreClear() {
		  foreach (var d in this) {
			  d.Dispose();
		  }
	  }
  }
