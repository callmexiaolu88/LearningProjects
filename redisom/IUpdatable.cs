namespace redisom
{
    public interface IUpdatable<T>
    {
        bool CanUpdate(T other);

        void Update(T other);
    }
}