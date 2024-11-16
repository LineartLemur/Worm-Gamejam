using System;
using UniRx;

namespace Shared.ReactiveList {
    public class ReactiveConvertedProperty<C, O> : IReactiveProperty<C> {
        private IReactiveProperty<O> backingProperty;
        private Func<C, O> c2O;
        private Func<O, C> o2C;

        public ReactiveConvertedProperty(
            IReactiveProperty<O> backingProperty,
            Func<C, O> c2o,
            Func<O, C> o2c
        ) {
            this.backingProperty = backingProperty;
            this.c2O = c2o;
            this.o2C = o2c;
        }

        public IDisposable Subscribe(IObserver<C> observer) {
            return backingProperty.Subscribe(Observer.Create<O>(o => observer.OnNext(o2C(o)), observer.OnError,
                observer.OnCompleted));
        }

        public C Value {
            get => o2C(backingProperty.Value);
            set => backingProperty.Value = c2O(value);
        }

        public bool HasValue => backingProperty.HasValue;
    }
}