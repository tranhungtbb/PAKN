import { LoginsModule } from './logins.module';

describe('LoginsModule', () => {
  let loginsModule: LoginsModule;

  beforeEach(() => {
    loginsModule = new LoginsModule();
  });

  it('should create an instance', () => {
    expect(loginsModule).toBeTruthy();
  });
});
