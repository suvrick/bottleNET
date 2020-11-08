import { SlotVoid } from "../common/Slot";
import { ByteArray } from "./ByteArray";


export class Socket
{
	public get isActive(): boolean
	{
		return this.client && this.client.readyState === this.client.OPEN;
	}

	public get bytesAvailable(): number
	{
		return this.inputByteArray.length - this.inputByteArray.position;
	}

	public get bytesPending(): number
	{
		return this.outputByteArray.length - this.outputByteArray.position;
	}

	public get connected(): boolean
	{
		return this.client != null && this.client.readyState === WebSocket.OPEN;
	}

	public get endian(): string
	{
		return this._endian;
	}

	public set endian(value: string)
	{
		this._endian = value;
		this.inputByteArray.endian = value;
		this.outputByteArray.endian = value;
	}

	public client: WebSocket | any = null;

	public inputByteArray: ByteArray = new ByteArray();
	public outputByteArray: ByteArray = new ByteArray();

	private _endian: string = ByteArray.BIG_ENDIAN;

	public readonly onConnectionClosed: SlotVoid = new SlotVoid();
	public readonly onErrorOccured: SlotVoid = new SlotVoid();
	public readonly onDataReceived: SlotVoid = new SlotVoid();

	public readBytes(bytes: ByteArray, offset: number = 0, length: number = 0): void
	{
		this.inputByteArray.readBytes(bytes, offset, length);
	}

	public writeBytes(bytes: ByteArray, offset: number = 0, length: number = 0): void
	{
		this.outputByteArray.writeBytes(bytes, offset, length);
	}

	public writeBoolean(value: boolean): void
	{
		this.outputByteArray.writeBoolean(value);
	}

	public writeByte(value: number): void
	{
		this.outputByteArray.writeByte(value);
	}

	public writeShort(value: number): void
	{
		this.outputByteArray.writeShort(value);
	}

	public writeInt(value: number): void
	{
		this.outputByteArray.writeInt(value);
	}

	public writeUnsignedInt(value: number): void
	{
		this.outputByteArray.writeUnsignedInt(value);
	}

	public writeFloat(value: number): void
	{
		this.outputByteArray.writeFloat(value);
	}

	public writeDouble(value: number): void
	{
		this.outputByteArray.writeDouble(value);
	}

	public writeMultiByte(value: string, charSet: string): void
	{
		this.outputByteArray.writeMultiByte(value, charSet);
	}

	public writeUTF(value: string): void
	{
		this.outputByteArray.writeUTF(value);
	}

	public writeUTFBytes(value: string): void
	{
		this.outputByteArray.writeUTFBytes(value);
	}

	public readBoolean(): boolean
	{
		return this.inputByteArray.readBoolean();
	}

	public readByte(): number
	{
		return this.inputByteArray.readByte();
	}

	public readUnsignedByte(): number
	{
		return this.inputByteArray.readUnsignedByte();
	}

	public readShort(): number
	{
		return this.inputByteArray.readShort();
	}

	public readUnsignedShort(): number
	{
		return this.inputByteArray.readUnsignedShort();
	}

	public readInt(): number
	{
		return this.inputByteArray.readInt();
	}

	public readUnsignedInt(): number
	{
		return this.inputByteArray.readUnsignedInt();
	}

	public readFloat(): number
	{
		return this.inputByteArray.readFloat();
	}

	public readDouble(): number
	{
		return this.inputByteArray.readDouble();
	}

	public readMultiByte(length: number): string
	{
		return this.inputByteArray.readMultiByte(length);
	}

	public readUTF(): string
	{
		return this.inputByteArray.readUTF();
	}

	public readUTFBytes(length: number): string
	{
		return this.inputByteArray.readUTFBytes(length);
	}

	public flush(): void
	{
		let data: Uint8Array = new Uint8Array(this.outputByteArray.length);
		let count: number = this.outputByteArray.position;
		this.outputByteArray.position = 0;

		let i: number = 0;
		while (count-- > 0)
		{
			let d: number = this.outputByteArray.readByte();
			data[i++] = d;
		}

		this.outputByteArray.clear();

		if (this.connected)
			this.client.send(data.buffer);
	}

	public close(): void
	{
		this.internalClose();
	}

	public connect(host: string, port: number): Promise<void>
	{
		console.log("Connect to " + host + ":" + port);

		return new Promise((resolve, reject): void =>
		{
			this.client = new WebSocket(host + ":" + port);
			this.client.binaryType = "arraybuffer";

			this.client.onmessage = this.onMessage.bind(this);

			this.client.onclose = reject;
			this.client.onerror = reject;

			this.client.onopen = (): void =>
			{
				console.log("OPEN CONNECTION");
				this.client.onclose = this.onClose.bind(this);
				this.client.onerror = this.onError.bind(this);
				resolve();
			};
		});
	}

	private internalClose(): void
	{
		if (this.client != null)
			this.client.close();

		this.client = null;
	}

	private onClose(): void
	{
		console.log("CLOSED CONNECTION");
		this.outputByteArray.clear();
		this.onConnectionClosed.call();
	}

	private onError(): void
	{
		console.log("ERROR CONNECTION");
		this.outputByteArray.clear();
		this.onErrorOccured.call();
	}

	private onMessage(e: MessageEvent): void
	{
		if (this.bytesAvailable === 0)
			this.inputByteArray.clear();

		this.inputByteArray.setArrayBuffer(e.data);
		this.inputByteArray.position = 0;
		this.onDataReceived.call();
	}
}
