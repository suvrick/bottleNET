class ArrayBufferPool
{
	private static buffers: Uint8ClampedArray[][] = [];

	public static Add(buffer: Uint8ClampedArray): void
	{
		if (!(buffer.byteLength in ArrayBufferPool.buffers))
			ArrayBufferPool.buffers[buffer.byteLength] = [];

		ArrayBufferPool.buffers[buffer.byteLength].push(buffer);
	}

	public static Get(size: number): Uint8ClampedArray
	{
		if (!(size in ArrayBufferPool.buffers))
			ArrayBufferPool.buffers[size] = [];

		if (ArrayBufferPool.buffers[size].length === 0)
			return new Uint8ClampedArray(new ArrayBuffer(size));

		let len: number = ArrayBufferPool.buffers[size].length - 1;
		let remove: Uint8ClampedArray = ArrayBufferPool.buffers[size][len];
		ArrayBufferPool.buffers[size].splice(len, 1);
		return remove;
	}
}

export class ByteArray
{
	private static SIZE_OF_INT8: number = 1;
	private static readonly STEP: number = 65000;

	public static readonly BIG_ENDIAN: string = "bigEndian";
	public static readonly LITTLE_ENDIAN: string = "littleEndian";

	private dataView: any = null;
	private bufferU8: any = null;

	private _endian: boolean = false;
	private _length: number = 0;
	private _position: number = 0;

	public constructor(buffer: any = null)
	{
		if (buffer == null)
			this.bufferU8 = ArrayBufferPool.Get(1024);
		else
		{
			this.bufferU8 = new Uint8ClampedArray(buffer);
			this.length = buffer.byteLength;
		}

		this.dataView = new DataView(this.bufferU8.buffer);
	}

	public setArrayBuffer(newBuffer: ArrayBuffer): void	// internal
	{
		this.bufferU8 = new Uint8ClampedArray(newBuffer);
		this.position = 0;
		this.length = newBuffer.byteLength;

		this.dataView = new DataView(this.bufferU8.buffer);
	}

	public readBytes(bytes: ByteArray, offset: number = 0, length: number = 0): void
	{
		let savePos: number = bytes.position;
		bytes.position = offset;

		if (length === 0)
			length = this._length - offset;

		bytes.writeBytes(this, this.position, length);
		this.position += length;

		bytes.position = savePos;
	}

	public writeBytes(bytes: ByteArray, offset: number = 0, length: number = 0): void
	{
		if (length === 0)
			length = bytes.length - offset;

		let byteLength: number = this.bufferU8.byteLength >>> 0;
		if (this._length < this.position + length)
			this._length = this.position + length;

		while (byteLength < this._length)
			byteLength *= 2;

		if (byteLength === this.bufferU8.byteLength)
			this.bufferU8.set(new Uint8ClampedArray(bytes.bufferU8.buffer, offset, length), this.position);
		else
		{
			let buffer: Uint8ClampedArray = ArrayBufferPool.Get(byteLength);
			buffer.set(this.bufferU8);
			buffer.set(new Uint8ClampedArray(bytes.bufferU8.buffer, offset, length), this.position);
			ArrayBufferPool.Add(this.bufferU8);
			this.bufferU8 = buffer;

			this.dataView = new DataView(this.bufferU8.buffer);
		}

		this.position += length;
	}

	public writeBoolean(value: boolean): void
	{
		this.writeByte(value ? 1 : 0);
	}

	public writeByte(value: number): void
	{
		if (this._length < this._position + ByteArray.SIZE_OF_INT8)
			this._length = this._position + ByteArray.SIZE_OF_INT8;

		while (this._length >= (this.bufferU8.byteLength >>> 0))
			this.expand();

		this.dataView.setUint8(this.position, (value & 0xff));
		this.position++;
	}

	public writeUnsignedByte(value: number): void
	{
		this.write(1);
		this.dataView.setUint8(this.position, (value & 0xff));
		this.position++;
	}

	public writeShort(value: number): void
	{
		this.write(2);
		this.dataView.setInt16(this.position, value, this._endian);
		this.position += 2;
	}

	public writeInt(value: number): void
	{
		this.write(4);
		this.dataView.setInt32(this.position, value, this._endian);
		this.position += 4;
	}

	public writeUnsignedInt(value: number): void
	{
		this.write(4);
		this.dataView.setInt32(this.position, (value >>> 0), this._endian);
		this.position += 4;
	}

	public writeFloat(value: number): void
	{
		this.write(4);
		this.dataView.setFloat32(this.position, value, this._endian);
		this.position += 4;
	}

	public writeDouble(value: number): void
	{
		this.write(8);
		this.dataView.setFloat64(this.position, value, this._endian);
		this.position += 8;
	}

	public writeMultiByte(value: string, charSet: string): void
	{
		if (!(charSet === ""))
			throw new Error();

		this.writeUTFBytes(value);
	}

	public writeUTF(value: string): void
	{
		this.writeShort(unescape(encodeURIComponent(value)).length);
		this.writeUTFBytes(value);
	}

	public writeUTFBytes(value: string): void
	{
		value = unescape(encodeURIComponent(value));

		let i: number = 0;
		let l: number = value.length;

		for (; i < l - 10; i += 10)
		{
			this.writeByte(value.charCodeAt(i));
			this.writeByte(value.charCodeAt(i + 1));
			this.writeByte(value.charCodeAt(i + 2));
			this.writeByte(value.charCodeAt(i + 3));
			this.writeByte(value.charCodeAt(i + 4));
			this.writeByte(value.charCodeAt(i + 5));
			this.writeByte(value.charCodeAt(i + 6));
			this.writeByte(value.charCodeAt(i + 7));
			this.writeByte(value.charCodeAt(i + 8));
			this.writeByte(value.charCodeAt(i + 9));
		}

		for (; i < l; i++)
			this.writeByte(value.charCodeAt(i));
	}

	public readBoolean(): boolean
	{
		return this.readByte() > 0;
	}

	public readByte(): number
	{
		if (this.position + ByteArray.SIZE_OF_INT8 > this._length)
			throw new Error("Failed to read past end of the stream");

		let r: number = this.dataView.getInt8(this.position);
		this._position++;
		return r;
	}

	public readUnsignedByte(): number
	{
		if (this.position + ByteArray.SIZE_OF_INT8 > this._length)
			throw new Error("Failed to read past end of the stream");

		let r: number = this.dataView.getUint8(this.position);
		this._position++;
		return r;
	}

	public readShort(): number
	{
		let r: number = this.dataView.getInt16(this.position, this._endian);
		this._position += 2;
		return r;
	}

	public readUnsignedShort(): number
	{
		let r: number = this.dataView.getUint16(this.position, this._endian);
		this._position += 2;
		return r;
	}

	public readInt(): number
	{
		let r: number = this.dataView.getInt32(this.position, this._endian);
		this._position += 4;
		return r;
	}

	public readUnsignedInt(): number
	{
		let r: number = this.dataView.getUint32(this.position, this._endian);
		this._position += 4;
		return r;
	}

	public readFloat(): number
	{
		let r: number = this.dataView.getFloat32(this.position, this._endian);
		this._position += 4;
		return r;
	}

	public readDouble(): number
	{
		let r: number = this.dataView.getFloat64(this.position, this._endian);
		this._position += 8;
		return r;
	}

	public readMultiByte(length: number): string
	{
		return this.readUTFBytes(length);
	}

	public readUTF(): string
	{
		return this.readUTFBytes(this.readUnsignedShort());
	}

	public readUTFBytes(length: number): string
	{
		let result: string = "";
		//@ts-ignore
		let str: string = null;
		let step: number = ByteArray.STEP;
		let _length: number = (length + this.position);
		for (let i = this.position; i < _length; i += step)
		{
			let size: number = Math.min(step, _length - i);
			//@ts-ignore
			let s: string = String.fromCharCode.apply(String, new Uint8ClampedArray(this.bufferU8.buffer, i, size));
			if (str == null)
				str = s;
			else
				str += s;
		}
		if (str == null)
			str = "";


		result = decodeURIComponent(escape(str));

		this._position += length;
		return result;
	}

	public toString(): string
	{
		return this._toString();
	}

	public ToString(): string
	{
		return "[BA: Length:" + this.length + "]";
	}

			
	public clear(): void
	{
		this.length = 0;
		this.position = 0;
	}

	public get(key: number): number
	{
		return this.bufferU8[key];
	}

	public set(key: number, value: number): void
	{
		if (key === this._length)
		{
			this._length++;
			this._position++;
		}

		while (this._length >= this.bufferU8.byteLength)
			this.expand();

		this.bufferU8[key] = value;
	}

	//@ts-ignore
	public get length(): number
	{
		return this._length;
	}

	//@ts-ignore
	public set length(value: number)
	{
		this._length = value;
	}

	//@ts-ignore
	public get bytesAvailable(): number
	{
		return this.length - this.position;
	}

	//@ts-ignore
	public get position(): number
	{
		return this._position;
	}

	//@ts-ignore
	public set position(value: number)
	{
		this._position = value;
	}


	public get endian(): string
	{
		return this._endian ? ByteArray.LITTLE_ENDIAN : ByteArray.BIG_ENDIAN;
	}


	public set endian(value: string)
	{
		this._endian = (value === ByteArray.LITTLE_ENDIAN);
	}

	private expand(): void
	{
		let newBuffer: Uint8ClampedArray = ArrayBufferPool.Get(this.bufferU8.byteLength * 2);
		newBuffer.set(this.bufferU8);

		ArrayBufferPool.Add(this.bufferU8);
		this.bufferU8 = newBuffer;

		this.dataView = new DataView(this.bufferU8.buffer);
	}

	private write(length: number): void
	{
		if (this._length < this._position + (length >>> 0))
			this._length = this._position + (length >>> 0);

		while (this._length >= this.bufferU8.byteLength)
			this.expand();
	}

	private fromCharCode(): string
	{
		let str: string = "";
		let step: number = ByteArray.STEP;
		for (let i = 0; i < this.length; i += step)
		{
			let size: number = Math.min(step, this.length - i);
			//@ts-ignore
			str += String.fromCharCode.apply(String, new Uint8ClampedArray(this.bufferU8.buffer, i, size));
		}
		return str;
	}

	private _toString(): string
	{
		let str: string = this.fromCharCode();
		try
		{
			return decodeURIComponent(escape(str));
		}
		catch (e)
		{
			return str;
		}
	}
}