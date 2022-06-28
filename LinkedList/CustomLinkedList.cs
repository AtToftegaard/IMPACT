using System.Text;

namespace LinkedList
{
    internal class CustomLinkedList<T>
    {
        Node<T> head;
        public CustomLinkedList()
        {
        }

        public CustomLinkedList(T data)
        {
            Node<T> head = new Node<T>(data, default);
        }

        public void Add(T data)
        {
            Node<T> toAdd = new(data, head);
            if(head != null)
            {
                head.Next = toAdd;
                toAdd.Previous = head;
            }
            head = toAdd;
        }

        public override string ToString()
        {
            var current = head;

            static StringBuilder f(Node<T> node)
            {
                StringBuilder sb;
                if (node.Previous != null)
                {
                    var shouldHaveArrow = node.Next == null ? "" : " -> ";
                    sb = f(node.Previous).Append(node.data).Append(shouldHaveArrow);
                } else
                {
                    return new StringBuilder($"{node.data} -> ");
                }
                return sb;
            }
            var sb = f(current);
            return sb.ToString();
        }
    }

    internal class Node<T>
    {
        public Node(T value, Node<T> previous)
        {
            data = value;
        }

        public Node<T>? Next;
        public Node<T>? Previous;
        public readonly T data;
    }
}
