namespace TrainingCenter.Api.Entities;

public enum EnrollmentStatus { Pending, Active, Completed, Cancelled }

public enum TrackLevel { Beginner, Intermediate, Advanced }

public enum TrackStatus { Open, Closed, Completed }

public enum PaymentStatus { Pending, Paid, Failed, Refunded }

public enum PaymentMethod { Cash, Card, BankTransfer, Online }
