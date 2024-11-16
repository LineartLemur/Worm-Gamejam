
public interface IInitable<T> {
    void Init(T data);
}

public interface IInitable<T, C> {
    void Init(T data, C collection);
}