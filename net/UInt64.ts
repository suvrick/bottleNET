import { ByteArray } from "./ByteArray";

export class UInt64
{
	public high: number = 0;
	public low: number = 0;

	public constructor(buffer: ByteArray | string)
	{
		if (typeof buffer === "string")
			this.readString(buffer);
		else if (buffer instanceof ByteArray)
			this.readByteArray(buffer);
	}

	public write(buffer: ByteArray): void
	{
		buffer.writeUnsignedInt(this.low);
		buffer.writeUnsignedInt(this.high);
	}

	public toString(): string
	{
		if (this.high === 0)
			return this.low.toString();

		let result: string = "";
		let lowCopy: number = this.low;
		let highCopy: number = this.high;

		while (highCopy !== 0)
		{
			let left: number = highCopy % 10;
			highCopy = (highCopy / 10) >>> 0;

			lowCopy += left * 0x100000000;

			result = String(lowCopy % 10) + result;
			lowCopy = (lowCopy / 10) >>> 0;
		}

		return lowCopy.toString() + result;
	}

	private readByteArray(buffer: ByteArray): void
	{
		this.low = buffer.readUnsignedInt();
		this.high = buffer.readUnsignedInt();
	}

	private readString(buffer: string): void
	{
		for (let i: number = 0; i < buffer.length; i++)
		{
			let sym: number = parseInt(buffer.charAt(i), 10);

			this.low *= 10;
			this.high *= 10;

			this.low += sym;

			if (this.low < 0xFFFFFFFF)
				continue;

			let left: number = (this.low / 0x100000000) >>> 0;
			this.high += left;

			this.low = (this.low) >>> 0;
		}
	}
}