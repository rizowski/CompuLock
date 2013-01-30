class AccountProcess < ActiveRecord::Base
  attr_accessible :lastrun, :name

  validates :name, :presence => true

  belongs_to :account
end
