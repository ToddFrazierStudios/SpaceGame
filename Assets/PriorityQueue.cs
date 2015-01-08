using System;
using System.Collections;

// Screw you C# for not having one and making me write one
public class PriorityQueue <T> where T:IComparable<T> {
	private T[] queue;
	private int max;
	private int count;

	public PriorityQueue(int maxSize) {
		queue = new T[maxSize];
		max = maxSize;
		count = 0;
	}

	public T pop() {
		if (count != 0) {
			T removed = queue[0];
			queue[0] = queue[count - 1];
			count--;
			siftDown(0);
			return removed;
		}
		return default(T);
	}

	public T peek() {
		return queue[0];
	}

	public void add(T item) {
		if (count == max) {
			return;
		}
		queue[count] = item;
		count++;
		siftUp (count - 1);
	}

	public void remove(T toRemove) {
		if (count != 0 && toRemove != null) {
			int i = 0;
			while (i < count - 1 && toRemove.CompareTo (queue[i]) != 0) {
				i++;
			}
			queue[i] = default(T);
			queue[i] = queue[count - 1];
			siftDown (i);
		}
	}

	public void rebuild() {
		PriorityQueue<T> newQueue = new PriorityQueue<T>(max);
		int tempCount = count;
		while (count > 0) {
			newQueue.add (this.pop ());
		}
		queue = newQueue.toArray();
		count = tempCount;
	}

	public T[] toArray() {
		return queue;
	}

	public int getCount() {
		return count;
	}

	private void siftUp(int i) {
		if (i == 0) {
			return;
		}
		T toSift = queue[i];
		int current = i;
		int parent;
		while (current > 0) {
			parent = (current - 1) / 2;
			if (queue[current].CompareTo (queue[parent]) <= 0) {
				break;
			}
			queue[current] = queue[parent];
			queue[parent] = toSift;
			current = parent;
		}
		queue[current] = toSift;
	}

	private void siftDown(int i) {
		T toSift = queue[i];
		int parent = i;
		int child = 2 * parent + 1;
		while (child < count) {
			if (child < count + 1 && queue[child].CompareTo (queue[child + 1]) < 0) {
				child = child + 1;
			}
			if (toSift.CompareTo (queue[child]) >= 0) {
				break;
			}
			queue[parent] = queue[child];
			queue[child] = toSift;
			parent = child;
			child = 2 * parent + 1;
		}
		queue[parent] = toSift;
	}
}
